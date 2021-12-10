namespace Common.Network
{
    using _EventArgs_;
    using Common.Network._Enums_;
    using Messages;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using WebSocketSharp.Server;


    public class WsServer
    {
        #region Fields

        private readonly IPEndPoint _listenAddress;
        private readonly ConcurrentDictionary<Guid, WsConnection> _connections;

        private WebSocketServer _server;
        private UsersListsManager _usersLists;

        #endregion Fields

        #region Events

        public event EventHandler<ConnectionStateChangedEventArgs> ConnectionStateChanged;
        public event EventHandler<MessageReceivedEventArgs> MessageReceived;
        public event EventHandler<CheckLoginEventArgs> CheckLogin;
        public event EventHandler<RegistrationRequestEventArgs> RegistrationRequestEvent;
        public event EventHandler<ListOfMessagesBroadcastEventArgs> ListOfMessagesBroadcast;
        public event EventHandler LoadUsersList;
        public event EventHandler<EventLogRequestEventArgs> EventLogRequestEvent;

        #endregion Events

        #region Constructors

        public WsServer(IPEndPoint listenAddress)
        {
            _listenAddress = listenAddress;
            _connections = new ConcurrentDictionary<Guid, WsConnection>();
        }

        #endregion Constructors

        #region Methods

        public void Start()
        {
            _server = new WebSocketServer(_listenAddress.Address, _listenAddress.Port, false);
            _server.AddWebSocketService<WsConnection>("/",
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

            var connections = _connections.Select(item => item.Value).ToArray();
            foreach (var connection in connections)
            {
                connection.Close();
            }
            _connections.Clear();
        }

        public void Send(string text, string sourceUser, string targetUser)
        {
            Message nonJsonMessage = new Message { Text = text, UsernameSource = sourceUser, UsernameTarget = targetUser, Time = DateTime.Now };
            var message = JsonConvert.SerializeObject(nonJsonMessage);
            var messageBroadcast = new MessageBroadcast(message).GetContainer();
            if (targetUser == "Global")
            {
                foreach (var connection in _connections)
                {
                    connection.Value.Send(messageBroadcast);
                }
            }
            else
            {
                if (_usersLists.IsUserOnline(targetUser))
                {
                    var guidTarget = _usersLists.GetUserGuid(targetUser);
                    var connectionTarget = _connections.FirstOrDefault(x => x.Key == guidTarget).Value;
                    connectionTarget.Send(messageBroadcast);
                }

                if (targetUser != sourceUser)
                {
                    var guidSource = _usersLists.GetUserGuid(sourceUser);
                    var connectionSource = _connections.FirstOrDefault(x => x.Key == guidSource).Value;
                    connectionSource.Send(messageBroadcast);
                }
            }
        }

        public void Send(string message)
        {

            var messageBroadcast = new MessageBroadcast(message).GetContainer();

            foreach (var connection in _connections)
            {
                connection.Value.Send(messageBroadcast);
            }
        }

        internal void HandleMessage(Guid clientId, MessageContainer container)
        {
            if (!_connections.TryGetValue(clientId, out WsConnection connection))
                return;

            switch (container.Identifier)
            {
                case nameof(ConnectionRequest):
                    var connectionRequest = ((JObject)container.Payload).ToObject(typeof(ConnectionRequest)) as ConnectionRequest;

                    var connectionResponse = new ConnectionResponse { Result = ResultCodes.Ok };
                    if (_connections.Values.Any(item => item.Login == connectionRequest.Login))
                    {
                        connectionResponse.Result = ResultCodes.Failure;
                        connectionResponse.Reason = $"Клиент с именем '{connectionRequest.Login}' уже подключен.";
                        connection.Send(connectionResponse.GetContainer());
                    }
                    else
                    {
                        CheckLogin?.Invoke(this, new CheckLoginEventArgs(connectionRequest.Login,
                            connectionRequest.Password, connection, clientId, connectionResponse));
                    }
                    break;
                case nameof(RegistrationRequest):
                    var registrationRequest = ((JObject)container.Payload).ToObject(typeof(RegistrationRequest)) as RegistrationRequest;
                    if (_connections.Values.Any(item => item.Login == registrationRequest.Login))
                    {
                        var registrationResponse = new RegistrationResponse(RegistrationResult.UserAlreadyExists);
                        connection.Send(registrationResponse.GetContainer());
                    }
                    RegistrationRequestEvent?.Invoke(this, new RegistrationRequestEventArgs(registrationRequest.Login, registrationRequest.Password, connection));
                    break;
                case nameof(MessageRequest):
                    var messageRequest = ((JObject)container.Payload).ToObject(typeof(MessageRequest)) as MessageRequest;
                    MessageReceived?.Invoke(this, new MessageReceivedEventArgs(connection.Login, messageRequest.Message));
                    break;
                case nameof(ListOfMessagesRequest):
                    var listOfMessagesRequest = ((JObject)container.Payload).ToObject(typeof(ListOfMessagesRequest)) as ListOfMessagesRequest;
                    ListOfMessagesBroadcast?.Invoke(this, new ListOfMessagesBroadcastEventArgs(connection, listOfMessagesRequest.MyLogin, listOfMessagesRequest.CompanionLogin));
                    break;
                case nameof(EventLogRequest):
                    var eventLogRequest = ((JObject)container.Payload).ToObject(typeof(EventLogRequest)) as EventLogRequest;
                    EventLogRequestEvent?.Invoke(this, new EventLogRequestEventArgs(connection, eventLogRequest.FirstDate, eventLogRequest.SecondDate));
                    break;
            }
        }
        public void SendEventLog(WsConnection connection,List<EventNote> eventLog)
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
            _usersLists.LoadListFromDB(users);
        }
        public void CheckLoginResponse(string login, string password, WsConnection connection,
            Guid clientId, ConnectionResponse connectionResponse, LoginResult result)
        {
            if (result == LoginResult.Ok)
            {
                ConnectionStateChanged?.Invoke(this, new ConnectionStateChangedEventArgs(login, true, "Вход успешно выполнен"));
                connection.Login = login;
                _usersLists.AddToLists(login, clientId);

                connection.Send(connectionResponse.GetContainer());

                var usersStatuses = new UsersStatusesBroadcast(_usersLists.GetUsersStatuses()).GetContainer();
                connection.Send(usersStatuses);

                Console.WriteLine("Отправлен список");
                UserState newUser = new UserState(login, true); // отправка изменений о состоянии пользователя
                UserStatusChangeBroadcast newChange = new UserStatusChangeBroadcast(newUser);
                foreach (var connects in _connections)
                {
                    connects.Value.Send(newChange.GetContainer());
                }
                //ListOfMessagesBroadcast?.Invoke(this, new ListOfMessagesBroadcastEventArgs(connection, login, "Global"));
            }
            else if (result == LoginResult.UnknownUser)
            {
                connectionResponse.Result = ResultCodes.Failure;
                connectionResponse.Reason = $"Данный пользователь не зарегистрирован.";
                connection.Send(connectionResponse.GetContainer());
            }
            else if (result == LoginResult.UnknownPassword)
            {
                connectionResponse.Result = ResultCodes.Failure;
                connectionResponse.Reason = $"Неправильный пароль.";
                connection.Send(connectionResponse.GetContainer());
            }
            else
            {
                connectionResponse.Result = ResultCodes.Failure;
                connectionResponse.Reason = $"Неизвестная ошибка.";
                connection.Send(connectionResponse.GetContainer());
            }

        }

        internal void AddConnection(WsConnection connection)
        {
            _connections.TryAdd(connection.Id, connection);
        }

        internal void FreeConnection(Guid connectionId)
        {
            // отправка изменений о состоянии пользователя
            UserState newUser = new UserState(_usersLists.GetUserName(connectionId), false);
            UserStatusChangeBroadcast newChange = new UserStatusChangeBroadcast(newUser);
            foreach (var connects in _connections)
            {
                connects.Value.Send(newChange.GetContainer());
            }
            _usersLists.DeleteFromLists(connectionId);

            if (_connections.TryRemove(connectionId, out WsConnection connection) && !string.IsNullOrEmpty(connection.Login))
                ConnectionStateChanged?.Invoke(this, new ConnectionStateChangedEventArgs(connection.Login, false, "Соединение принудительно разорвано"));
        }

        #endregion Methods
    }
}
