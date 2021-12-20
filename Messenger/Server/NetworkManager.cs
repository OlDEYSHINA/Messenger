namespace Server
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Net;

    using Common.Network;
    using Common.Network.Messages;

    using Database;
    using Database.DBModels;
    using Database.DBServices;

    using Newtonsoft.Json;

    using Settings;

    public class NetworkManager
    {
        #region Fields

        private readonly long _timeout;
        private readonly int _port;
        private readonly IPAddress _ip;

        private readonly WsServer _wsServer;

        private readonly UserService _userService;
        private readonly MessageService _messageService;
        private readonly ClientEventService _clientEventService;

        #endregion

        #region Constructors

        public NetworkManager()
        {
            DatabaseController databaseController;
            var settingsManager = new SettingsManager();
            _ip = settingsManager.Ip;
            _port = settingsManager.Port;
            _timeout = settingsManager.Timeout;
            ConnectionStringSettings connectionString = settingsManager.ConnectionSettings;

            try
            {
                databaseController = new DatabaseController(connectionString);
            }
            catch (Exception e)
            {
                Console.WriteLine("Ошибка подключения к БД по причине:\n" + e.Message + " \nЗапуск со стандартными настройками БД");
                var connectionSettings = new ConnectionStringSettings(
                    settingsManager.DefaultSettings.DbName,
                    settingsManager.DefaultSettings.ConnectionString,
                    settingsManager.DefaultSettings.ProviderName);
                databaseController = new DatabaseController(connectionSettings);
            }

            _wsServer = new WsServer(new IPEndPoint(IPAddress.Any, _port));
            _wsServer.Timeout = _timeout;
            _wsServer.ConnectionStateChanged += HandleConnectionStateChanged;
            _wsServer.MessageReceived += HandleMessageReceived;
            _wsServer.CheckLogin += HandleCheckLogin;
            _wsServer.RegistrationRequestEvent += HandleRegistrationRequest;
            _wsServer.ListOfMessagesBroadcast += HandleListOfMessagesBroadcast;
            _wsServer.LoadUsersList += HandleLoadUsersList;
            _wsServer.EventLogRequestEvent += HandleEventLogRequest;
            _userService = new UserService(databaseController);
            _messageService = new MessageService(databaseController);
            _clientEventService = new ClientEventService(databaseController);
        }

        #endregion

        #region Methods

        public void Start()
        {
            Console.WriteLine($"WebSocketServer: {IPAddress.Any}:{_port}");
            _wsServer.Start();
        }

        public void Stop()
        {
            _wsServer.Stop();
        }

        private void HandleEventLogRequest(object sender, EventLogRequestEventArgs e)
        {
            List<EventNote> eventLog = _clientEventService.GetEventLog(e.FirstDate, e.SecondDate);
            _wsServer.SendEventLog(e.Connection, eventLog);
        }

        private void HandleLoadUsersList(object sender, EventArgs e)
        {
            List<string> users = _userService.GetUsers();
            _wsServer.LoadUsersListResponse(users);
        }

        private void HandleListOfMessagesBroadcast(object sender, ListOfMessagesBroadcastEventArgs e)
        {
            List<Message> result;

            if (e.CompanionLogin == "Global")
            {
                result = _messageService.GetGlobalMessages();
            }
            else
            {
                result = _messageService.GetPrivateMessages(e.MyLogin, e.CompanionLogin);
            }

            var sendingList = new List<Common.Message>();

            foreach (Message msg in result)
            {
                var message = new Common.Message
                              {
                                  UsernameSource = msg.SourceUsername,
                                  UsernameTarget = msg.TargetUsername,
                                  Text = msg.MessageText,
                                  Time = msg.Date
                              };
                sendingList.Add(message);
            }

            var listOfMessages = new ListOfMessages(sendingList);
            _wsServer.SendListOfMessages(e.Connection, listOfMessages);
        }

        private void HandleRegistrationRequest(object sender, RegistrationRequestEventArgs e)
        {
            bool result = false;

            if (e.Login != "Global")
            {
                result = _userService.AddUser(e.Login, e.Password);
            }

            if (result)
            {
                _wsServer.RegistrationResponse(e.Connection, RegistrationResult.Ok);
                bool tryAddEventResult = _clientEventService.TryAddClientEvent(
                    "Registration module",
                    "Пользователь " + e.Login + " зарегистрировался",
                    DateTime.Now);

                if (!tryAddEventResult)
                {
                    Console.WriteLine("Ошибка записи в журнал событий");
                }
            }
            else
            {
                _wsServer.RegistrationResponse(e.Connection, RegistrationResult.UserAlreadyExists);
                bool otvet = _clientEventService.TryAddClientEvent(
                    "Registration module",
                    "Попытка зарегистрировать пользователя " + e.Login,
                    DateTime.Now);

                if (!otvet)
                {
                    Console.WriteLine("Ошибка записи в журнал событий");
                }
            }
        }

        private void HandleCheckLogin(object sender, CheckLoginEventArgs e)
        {
            LoginResult result = _userService.TryLogin(e.Login, e.Password);

            _wsServer.CheckLoginResponse(e.Login, e.Password, e.Connection, e.ClientId, e.ConnectionResponse, result);
        }

        private void HandleMessageReceived(object sender, MessageReceivedEventArgs e)
        {
            string message = $"Клиент '{e.ClientName}' отправил сообщение '{e.Message}'.";
            Console.WriteLine(message);
            var income = JsonConvert.DeserializeObject<Common.Message>(e.Message);
            _wsServer.Send(income.Text, income.UsernameSource, income.UsernameTarget);
            _messageService.TryAddMessage(income.UsernameSource, income.UsernameTarget, income.Text, DateTime.Now);
        }

        private void HandleConnectionStateChanged(object sender, ConnectionStateChangedEventArgs e)
        {
            string clientState = e.Connected ? "подключен" : "отключен";
            string message = $"Клиент '{e.ClientName}' {clientState} по причине'{e.Reason}'.";
            bool otvet = _clientEventService.TryAddClientEvent(e.ClientName, clientState, DateTime.Now);

            if (!otvet)
            {
                Console.WriteLine("Ошибка записи в журнал событий");
            }

            Console.WriteLine(message);
            _wsServer.Send(message);
        }

        #endregion
    }
}
