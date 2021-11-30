namespace Common.Network
{
    using _Enums_;
    using _EventArgs_;
    using Messages;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;
    using System;
    using System.Collections.Concurrent;
    using System.Threading;
    using WebSocketSharp;

    public class WsClient : ITransport
    {
        #region Fields

        private readonly ConcurrentQueue<MessageContainer> _sendQueue;

        private WebSocket _socket;

        private int _sending;
        private string _login;

        #endregion Fields

        #region Properties

        public bool IsConnected => _socket?.ReadyState == WebSocketState.Open;

        #endregion Properties

        #region Events

        public event EventHandler<ConnectionStateChangedEventArgs> ConnectionStateChanged;
        public event EventHandler<MessageReceivedEventArgs> MessageReceived;
        public event EventHandler<UsersStatusesReceivedEventArgs> UsersStatusesReceived;
        public event EventHandler<UserStateChangedEventArgs> UserStateChanged;
        public event EventHandler<RegistrationResponseReceivedEventArgs> RegistrationResponseReceived;
        public event EventHandler<LoginResponseReceivedEventArgs> LoginResponseReceived;

        #endregion Events

        #region Constructors

        public WsClient()
        {
            _sendQueue = new ConcurrentQueue<MessageContainer>();
            _sending = 0;
        }

        #endregion Constructors

        #region Methods

        public void Connect(string address, string port)
        {
            if (IsConnected)
                Disconnect();

            _socket = new WebSocket($"ws://{address}:{port}");
            _socket.OnOpen += OnOpen;
            _socket.OnClose += OnClose;
            _socket.OnMessage += OnMessage;
            _socket.ConnectAsync();
        }

        public void Disconnect()
        {
            if (_socket == null)
                return;

            if (IsConnected)
                _socket.CloseAsync();

            _socket.OnOpen -= OnOpen;
            _socket.OnClose -= OnClose;
            _socket.OnMessage -= OnMessage;

            _socket = null;
            _login = string.Empty;
        }

        public void Login(string login, string password)
        {
            _login = login;
            _sendQueue.Enqueue(new ConnectionRequest(_login, password).GetContainer());

            if (Interlocked.CompareExchange(ref _sending, 1, 0) == 0)
                SendImpl();
        }

        public void Registration(string login, string password)
        {
            _sendQueue.Enqueue(new RegistrationRequest(login, password).GetContainer());

            if (Interlocked.CompareExchange(ref _sending, 1, 0) == 0)
                SendImpl();
        }

        public void Send(string message)
        {
            _sendQueue.Enqueue(new MessageRequest(message).GetContainer());

            if (Interlocked.CompareExchange(ref _sending, 1, 0) == 0)
                SendImpl();
        }


        private void SendCompleted(bool completed)
        {
            // При отправке произошла ошибка.
            if (!completed)
            {
                Disconnect();
                ConnectionStateChanged?.Invoke(this, new ConnectionStateChangedEventArgs(_login,
                    false, "Ошибка отправки сообщения"));
                return;
            }

            SendImpl();
        }

        private void SendImpl()
        {
            if (!IsConnected)
                return;

            if (!_sendQueue.TryDequeue(out var message) && Interlocked.CompareExchange(ref _sending, 0, 1) == 1)
                return;

            var settings = new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore };
            string serializedMessages = JsonConvert.SerializeObject(message, settings);
            _socket.SendAsync(serializedMessages, SendCompleted);
        }

        private void OnMessage(object sender, MessageEventArgs e)
        {
            if (!e.IsText)
                return;

            var container = JsonConvert.DeserializeObject<MessageContainer>(e.Data);

            switch (container.Identifier)
            {
                case nameof(ConnectionResponse):
                    var connectionResponse = ((JObject)container.Payload).ToObject(typeof(ConnectionResponse)) as ConnectionResponse;
                    if (connectionResponse.Result == ResultCodes.Failure)
                    {

                        _login = string.Empty;
                        MessageReceived?.Invoke(this, new MessageReceivedEventArgs(_login, connectionResponse.Reason));
                    }
                    ConnectionStateChanged?.Invoke(this, new ConnectionStateChangedEventArgs(_login, true,connectionResponse.Reason ));
                    break;
                case nameof(MessageBroadcast):
                    var messageBroadcast = ((JObject)container.Payload).ToObject(typeof(MessageBroadcast)) as MessageBroadcast;
                    MessageReceived?.Invoke(this, new MessageReceivedEventArgs(_login, messageBroadcast.Message));
                    break;
                case nameof(UsersStatusesBroadcast):
                    var usersStatusBroadcast = ((JObject)container.Payload).ToObject(typeof(UsersStatusesBroadcast)) as UsersStatusesBroadcast;
                    UsersStatusesReceived?.Invoke(this, new UsersStatusesReceivedEventArgs(usersStatusBroadcast.ListOfUsersStatuses));
                    break;
                case nameof(UserStatusChangeBroadcast):
                    var userStatusBroadcast = ((JObject)container.Payload).ToObject(typeof(UserStateChangedEventArgs)) as UserStateChangedEventArgs;
                    UserStateChanged?.Invoke(this, new UserStateChangedEventArgs(userStatusBroadcast.user));
                    break;
                case nameof(RegistrationResponse):
                    var registrationResponse = ((JObject)container.Payload).ToObject(typeof(RegistrationResponse)) as RegistrationResponse;
                    RegistrationResponseReceived?.Invoke(this, new RegistrationResponseReceivedEventArgs(registrationResponse.RegistrationResult));
                    break;
                case nameof(LoginResponse):
                    var loginResponse = ((JObject)container.Payload).ToObject(typeof(LoginResponse)) as LoginResponse;
                    LoginResponseReceived?.Invoke(this, new LoginResponseReceivedEventArgs(loginResponse.LoginResult));
                    break;
            }
        }

        private void OnClose(object sender, CloseEventArgs e)
        {
            ConnectionStateChanged?.Invoke(this, new ConnectionStateChangedEventArgs(_login, false,"Сервер прекратил работу"));
        }

        private void OnOpen(object sender, System.EventArgs e)
        {
            ConnectionStateChanged?.Invoke(this, new ConnectionStateChangedEventArgs(_login, true,"Соединение установлено"));
        }

        #endregion Methods
    }
}
