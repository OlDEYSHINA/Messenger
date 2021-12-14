using Common;
using Common.Network;
using Common.Network._Enums_;
using Common.Network._EventArgs_;
using Common.Network.Messages;
using Newtonsoft.Json;
using Server.Database;
using Server.Database.DBServices;
using Server.Settings;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net;

namespace Server
{
    public class NetworkManager
    {
        private readonly long _timeout;
        private readonly int _port;
        private readonly IPAddress _ip;
        private readonly ConnectionStringSettings _connectionString;
       // private const int WS_PORT = 65000;

        private readonly WsServer _wsServer;

        private UserService _userService;
        private MessageService _messageService;
        private ClientEventService _clientEventService;
        private DatabaseController databaseController;

        public NetworkManager()
        {
            SettingsManager settingsManager = new SettingsManager();
            _ip = settingsManager.Ip;
            _port = settingsManager.Port;
            _timeout = settingsManager.Timeout;
            _connectionString = settingsManager.ConnectionSettings;
            try
            {
                databaseController = new DatabaseController(_connectionString);

            }
            catch(Exception e)
            {
                Console.WriteLine("Ошибка подключения к БД по причине:\n"+e.Message +" \nЗапуск со стандартными настройками БД");
                var connectionSettings = new ConnectionStringSettings(settingsManager.DefaultSettings.DBName,
                    settingsManager.DefaultSettings.ConnectionString, settingsManager.DefaultSettings.ProviderName);
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
        private void HandleEventLogRequest(object sender, EventLogRequestEventArgs e)
        {
            var eventLog = _clientEventService.GetEventLog(e.FirstDate,e.SecondDate);
            _wsServer.SendEventLog(e.Connection, eventLog);
        }
        private void HandleLoadUsersList(object sender, EventArgs e)
        {
            var users = _userService.GetUsers();
            _wsServer.LoadUsersListResponse(users);
        }
        private void HandleListOfMessagesBroadcast(object sender, ListOfMessagesBroadcastEventArgs e)
        {
            List<Database.DBModels.Message> result;
            if (e.CompanionLogin == "Global")
            {
                result = _messageService.GetGlobalMessages();
            }
            else
            {
                result = _messageService.GetPrivateMessages(e.MyLogin, e.CompanionLogin);
            }
            List<Message> sendingList = new List<Message>();
            foreach (Database.DBModels.Message msg in result)
            {
                var message = new Message
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
            bool result=false;
            if (e.Login != "Global")
            {
                result = _userService.AddUser(e.Login, e.Password);
            }
            if (result)
            {
                _wsServer.RegistrationResponse(e.Connection, RegistrationResult.Ok);
                var otvet = _clientEventService.TryAddClientEvent("Registration module", "Пользователь "+e.Login+" зарегистрировался", DateTime.Now);
                if (!otvet)
                {
                    Console.WriteLine("Ошибка записи в журнал событий");
                }
            }
            else
            {
                _wsServer.RegistrationResponse(e.Connection, RegistrationResult.UserAlreadyExists);
                var otvet = _clientEventService.TryAddClientEvent("Registration module", "Попытка зарегистрировать пользователя " + e.Login, DateTime.Now);
                if (!otvet)
                {
                    Console.WriteLine("Ошибка записи в журнал событий");
                }
            }
        }
        private void HandleCheckLogin(object sender, CheckLoginEventArgs e)
        {
            var result = _userService.TryLogin(e.Login, e.Password);

            _wsServer.CheckLoginResponse(e.Login, e.Password, e.Connection, e.ClientId, e.ConnectionResponse, result);

        }
        public void Start()
        {
            Console.WriteLine($"WebSocketServer: {IPAddress.Any}:{_port}");
            _wsServer.Start();
        }

        public void Stop()
        {
            _wsServer.Stop();
        }

        private void HandleMessageReceived(object sender, MessageReceivedEventArgs e)
        {
            string message = $"Клиент '{e.ClientName}' отправил сообщение '{e.Message}'.";
            Console.WriteLine(message);
            var income = JsonConvert.DeserializeObject<Message>(e.Message);
            _wsServer.Send(income.Text, income.UsernameSource, income.UsernameTarget);
            _messageService.TryAddMessage(income.UsernameSource, income.UsernameTarget, income.Text, DateTime.Now);
        }

        private void HandleConnectionStateChanged(object sender, ConnectionStateChangedEventArgs e)
        {
            string clientState = e.Connected ? "подключен" : "отключен";
            string message = $"Клиент '{e.ClientName}' {clientState} по причине'{e.Reason}'.";
            var otvet =_clientEventService.TryAddClientEvent(e.ClientName, clientState, DateTime.Now);
            if (!otvet)
            {
                Console.WriteLine("Ошибка записи в журнал событий");
            }
            Console.WriteLine(message);
            _wsServer.Send(message);
        }
    }
}
