﻿namespace Common.Network
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Timers;

    using Messages;

    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;

    using WebSocketSharp.Server;

    public class WsServer
    {
        #region Fields

        private readonly IPEndPoint _listenAddress;
        private readonly ConcurrentDictionary<Guid, WsConnection> _connections;

        private WebSocketServer _server;
        private readonly Dictionary<Guid, long> _timeoutClients;
        private readonly Timer _timeoutTimer;
        private UsersListsManager _usersLists;

        #endregion

        #region Properties

        public long Timeout { get; set; }

        #endregion

        #region Events

        public event EventHandler<ConnectionStateChangedEventArgs> ConnectionStateChanged;

        public event EventHandler<MessageReceivedEventArgs> MessageReceived;

        public event EventHandler<CheckLoginEventArgs> CheckLogin;

        public event EventHandler<RegistrationRequestEventArgs> RegistrationRequestEvent;

        public event EventHandler<ListOfMessagesBroadcastEventArgs> ListOfMessagesBroadcast;

        public event EventHandler LoadUsersList;

        public event EventHandler<EventLogRequestEventArgs> EventLogRequestEvent;

        #endregion

        #region Constructors

        public WsServer(IPEndPoint listenAddress)
        {
            _listenAddress = listenAddress;
            _connections = new ConcurrentDictionary<Guid, WsConnection>();
            _timeoutClients = new Dictionary<Guid, long>();
            _timeoutTimer = new Timer();
            _timeoutTimer.AutoReset = true;
            _timeoutTimer.Interval = 10000;
            _timeoutTimer.Elapsed += OnTimeoutEvent;
            _timeoutTimer.Enabled = true;
            _timeoutTimer.Start();
        }

        #endregion

        #region Methods

        public void Start()
        {
            _server = new WebSocketServer(_listenAddress.Address, _listenAddress.Port, false);
            _server.AddWebSocketService<WsConnection>(
                "/",
                client =>
                {
                    client.AddServer(this);
                });
            _usersLists = new UsersListsManager();
            LoadUsersList?.Invoke(this, null);

            _server.Start();
        }

        public void Stop()
        {
            _server?.Stop();
            _server = null;

            WsConnection[] connections = _connections.Select(item => item.Value).ToArray();

            foreach (WsConnection connection in connections)
            {
                connection.Close();
            }

            _timeoutClients.Clear();
            _connections.Clear();
        }

        public void Send(string text, string sourceUser, string targetUser)
        {
            var nonJsonMessage = new Message
                                 {
                                     Text = text,
                                     UsernameSource = sourceUser,
                                     UsernameTarget = targetUser,
                                     Time = DateTime.Now
                                 };
            string message = JsonConvert.SerializeObject(nonJsonMessage);
            MessageContainer messageBroadcast = new MessageBroadcast(message).GetContainer();

            if (targetUser == "Global")
            {
                foreach (KeyValuePair<Guid, WsConnection> connection in _connections)
                {
                    connection.Value.Send(messageBroadcast);
                }
            }
            else
            {
                if (_usersLists.IsUserOnline(targetUser))
                {
                    Guid guidTarget = _usersLists.GetUserGuid(targetUser);
                    WsConnection connectionTarget = _connections.FirstOrDefault(x => x.Key == guidTarget).Value;
                    connectionTarget.Send(messageBroadcast);
                }

                if (targetUser != sourceUser)
                {
                    Guid guidSource = _usersLists.GetUserGuid(sourceUser);
                    WsConnection connectionSource = _connections.FirstOrDefault(x => x.Key == guidSource).Value;
                    connectionSource.Send(messageBroadcast);
                }
            }
        }

        public void Send(string message)
        {
            MessageContainer messageBroadcast = new MessageBroadcast(message).GetContainer();

            foreach (KeyValuePair<Guid, WsConnection> connection in _connections)
            {
                connection.Value.Send(messageBroadcast);
            }
        }

        public void SendEventLog(WsConnection connection, List<EventNote> eventLog)
        {
            connection.Send(new EventLogResponse(eventLog).GetContainer());
        }

        public void SendListOfMessages(WsConnection connection, ListOfMessages listOfMessages)
        {
            connection.Send(listOfMessages.GetContainer());
        }

        public void RegistrationResponse(WsConnection connection, RegistrationResult result)
        {
            var registrationResponse = new RegistrationResponse(result);
            connection.Send(registrationResponse.GetContainer());
        }

        public void LoadUsersListResponse(List<string> users)
        {
            _usersLists.LoadListFromDb(users);
        }

        public void CheckLoginResponse(
            string login,
            string password,
            WsConnection connection,
            Guid clientId,
            ConnectionResponse connectionResponse,
            LoginResult result)
        {
            if (result == LoginResult.Ok)
            {
                ConnectionStateChanged?.Invoke(this, new ConnectionStateChangedEventArgs(login, true, "Вход успешно выполнен"));
                connection.Login = login;
                _usersLists.AddToLists(login, clientId);

                connection.Send(connectionResponse.GetContainer());

                MessageContainer usersStatuses = new UsersStatusesBroadcast(_usersLists.GetUsersStatuses()).GetContainer();
                connection.Send(usersStatuses);

                Console.WriteLine("Отправлен список");
                var newUser = new UserState(login, true); // отправка изменений о состоянии пользователя
                var newChange = new UserStatusChangeBroadcast(newUser);

                foreach (KeyValuePair<Guid, WsConnection> connects in _connections)
                {
                    connects.Value.Send(newChange.GetContainer());
                }
            }
            else if (result == LoginResult.UnknownUser)
            {
                connectionResponse.Result = ResultCodes.Failure;
                connectionResponse.Reason = "Данный пользователь не зарегистрирован.";
                connection.Send(connectionResponse.GetContainer());
            }
            else if (result == LoginResult.UnknownPassword)
            {
                connectionResponse.Result = ResultCodes.Failure;
                connectionResponse.Reason = "Неправильный пароль.";
                connection.Send(connectionResponse.GetContainer());
            }
            else
            {
                connectionResponse.Result = ResultCodes.Failure;
                connectionResponse.Reason = "Неизвестная ошибка.";
                connection.Send(connectionResponse.GetContainer());
            }
        }

        internal void HandleMessage(Guid clientId, MessageContainer container)
        {
            if (!_connections.TryGetValue(clientId, out WsConnection connection))
            {
                return;
            }

            _timeoutClients[clientId] = DateTime.Now.Ticks;

            switch (container.Identifier)
            {
                case nameof(ConnectionRequest):
                    var connectionRequest = ((JObject) container.Payload).ToObject(typeof(ConnectionRequest)) as ConnectionRequest;

                    var connectionResponse = new ConnectionResponse
                                             {
                                                 Result = ResultCodes.Ok
                                             };

                    if (_connections.Values.Any(item => item.Login == connectionRequest.Login))
                    {
                        connectionResponse.Result = ResultCodes.Failure;
                        connectionResponse.Reason = $"Клиент с именем '{connectionRequest.Login}' уже подключен.";
                        connection.Send(connectionResponse.GetContainer());
                    }
                    else
                    {
                        CheckLogin?.Invoke(
                            this,
                            new CheckLoginEventArgs(
                                connectionRequest.Login,
                                connectionRequest.Password,
                                connection,
                                clientId,
                                connectionResponse));
                    }

                    break;
                case nameof(RegistrationRequest):
                    var registrationRequest = ((JObject) container.Payload).ToObject(typeof(RegistrationRequest)) as RegistrationRequest;

                    if (_connections.Values.Any(item => item.Login == registrationRequest.Login))
                    {
                        var registrationResponse = new RegistrationResponse(RegistrationResult.UserAlreadyExists);
                        connection.Send(registrationResponse.GetContainer());
                    }

                    RegistrationRequestEvent?.Invoke(
                        this,
                        new RegistrationRequestEventArgs(registrationRequest.Login, registrationRequest.Password, connection));

                    break;
                case nameof(MessageResponse):
                    var messageRequest = ((JObject) container.Payload).ToObject(typeof(MessageResponse)) as MessageResponse;
                    MessageReceived?.Invoke(this, new MessageReceivedEventArgs(connection.Login, messageRequest.Message));

                    break;
                case nameof(ListOfMessagesRequest):
                    var listOfMessagesRequest = ((JObject) container.Payload).ToObject(typeof(ListOfMessagesRequest)) as ListOfMessagesRequest;
                    ListOfMessagesBroadcast?.Invoke(
                        this,
                        new ListOfMessagesBroadcastEventArgs(connection, listOfMessagesRequest.MyLogin, listOfMessagesRequest.CompanionLogin));

                    break;
                case nameof(EventLogRequest):
                    var eventLogRequest = ((JObject) container.Payload).ToObject(typeof(EventLogRequest)) as EventLogRequest;
                    EventLogRequestEvent?.Invoke(
                        this,
                        new EventLogRequestEventArgs(connection, eventLogRequest.FirstDate, eventLogRequest.SecondDate));

                    break;
            }
        }

        internal void AddConnection(WsConnection connection)
        {
            _connections.TryAdd(connection.Id, connection);
            _timeoutClients.Add(connection.Id, DateTime.Now.Ticks);
        }

        internal void FreeConnection(Guid connectionId)
        {
            // отправка изменений о состоянии пользователя
            var newUser = new UserState(_usersLists.GetUserName(connectionId), false);
            var newChange = new UserStatusChangeBroadcast(newUser);

            foreach (KeyValuePair<Guid, WsConnection> connects in _connections)
            {
                connects.Value.Send(newChange.GetContainer());
            }

            _usersLists.DeleteFromLists(connectionId);
            _timeoutClients.Remove(connectionId);

            if (_connections.TryRemove(connectionId, out WsConnection connection) && !string.IsNullOrEmpty(connection.Login))
            {
                ConnectionStateChanged?.Invoke(this, new ConnectionStateChangedEventArgs(connection.Login, false, "Соединение разорвано сервером"));
            }

            string name = _usersLists.GetUserName(connectionId);
        }

        private void OnTimeoutEvent(object sender, ElapsedEventArgs e)
        {
            List<Guid> timeClients = _timeoutClients.Where(item => DateTime.Now.Ticks - item.Value >= Timeout).Select(item => item.Key).ToList();

            foreach (Guid client in timeClients)
            {
                _timeoutClients?.Remove(client);
                _connections[client]?.Close();
                string name = _usersLists.GetUserName(client);
                Console.WriteLine("Клиент " + '"' + name + '"' + " отключен по бездействию");
            }
        }

        #endregion
    }
}
