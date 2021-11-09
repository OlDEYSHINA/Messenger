// ---------------------------------------------------------------------------------------------------------------------------------------------------
// Copyright ElcomPlus LLC. All rights reserved.
// Author: Пальников М. С.
// ---------------------------------------------------------------------------------------------------------------------------------------------------

namespace Common.Network
{
    using _EventArgs_;
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
        private List<UserStatus> _usersStatuses = new List<UserStatus>();
        private List<User> _users = new List<User>();

        #endregion Fields

        #region Events

        public event EventHandler<ConnectionStateChangedEventArgs> ConnectionStateChanged;
        public event EventHandler<MessageReceivedEventArgs> MessageReceived;

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
            //_server.AddWebSocketService("/", () => new WsConnection(this));
            _server.AddWebSocketService<WsConnection>("/",
                client =>
                {
                    client.AddServer(this);
                });
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
        public void SendTo(Guid id,string message)
        {
            _connections.TryGetValue(id,out WsConnection connection);
            var messageBroadcast = new MessageBroadcast(message).GetContainer();
            connection.Send(messageBroadcast);
        }
        public void Send(string text, string sourceUser, string targetUser)
        {
            Message nonJsonMessage = new Message { Text = text, UsernameSource = sourceUser, UsernameTarget = targetUser, Time = DateTime.Now };
            var message = JsonConvert.SerializeObject(nonJsonMessage);
            var messageBroadcast = new MessageBroadcast(message).GetContainer();

            foreach (var connection in _connections)
            {
                connection.Value.Send(messageBroadcast);
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
                        connection.Login = connectionRequest.Login;
                        AddToLists(connection.Login, clientId);
                        connection.Send(connectionResponse.GetContainer());
                        ConnectionStateChanged?.Invoke(this, new ConnectionStateChangedEventArgs(connection.Login, true));
                        var usersStatuses = new UserStatusBroadcast(_usersStatuses).GetContainer();
                        connection.Send(usersStatuses);
                        Console.WriteLine("Отправлен список");
                    }
                    break;
                case nameof(MessageRequest):
                    var messageRequest = ((JObject)container.Payload).ToObject(typeof(MessageRequest)) as MessageRequest;
                    MessageReceived?.Invoke(this, new MessageReceivedEventArgs(connection.Login, messageRequest.Message));
                    break;
            }
        }

        internal void AddConnection(WsConnection connection)
        {
            _connections.TryAdd(connection.Id, connection);

        }

        internal void FreeConnection(Guid connectionId)
        {
            DeleteFromLists(connectionId);
            if (_connections.TryRemove(connectionId, out WsConnection connection) && !string.IsNullOrEmpty(connection.Login))
                ConnectionStateChanged?.Invoke(this, new ConnectionStateChangedEventArgs(connection.Login, false));
        }

        protected void AddToLists(string login,Guid id)
        {
            User addUser = new User(login, id);
            _users.Add(addUser);
            if (_usersStatuses.Find(x => x.Name == login) == null)
            {
                UserStatus userStatus = new UserStatus(login, true);
                _usersStatuses.Add(userStatus);
            }
            else
            {
                _usersStatuses.Find(x => x.Name == login).IsOnline = true;
            }
        }

        /// <summary>
        /// Удаление пользователя из листа сервера и смена состояния в листе клиента
        /// </summary>
        /// <param name="id">Guid пользователя</param>
        protected void DeleteFromLists(Guid id)
        {
            var findedUser = _users.Find(x => x.ID == id);
            _users.Remove(findedUser);
            _usersStatuses.Find(x => x.Name == findedUser.Name).IsOnline=false;
        }

        #endregion Methods
    }
}
