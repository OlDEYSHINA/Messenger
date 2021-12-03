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
    class NetworkManager
    {
        private readonly long _timeout;
        private readonly int _port;
        private readonly IPAddress _ip;
        private readonly ConnectionStringSettings _connectionString;
        private const int WS_PORT = 65000;

        private readonly WsServer _wsServer;

        private UserService _userService;
        private MessageService _messageService;

        public NetworkManager()
        {
            SettingsManager settingsManager = new SettingsManager();
            _ip = settingsManager.Ip;
            _port = settingsManager.Port;
            _timeout = settingsManager.Timeout;
            _connectionString = settingsManager.ConnectionSettings;
            DatabaseController databaseController = new DatabaseController(_connectionString);

            _wsServer = new WsServer(new IPEndPoint(IPAddress.Any, WS_PORT));
            _wsServer.ConnectionStateChanged += HandleConnectionStateChanged;
            _wsServer.MessageReceived += HandleMessageReceived;
            _wsServer.CheckLogin += HandleCheckLogin;
            _wsServer.RegistrationRequestEvent += HandleRegistrationRequest;
            _wsServer.ListOfMessagesBroadcast += HandleListOfMessagesBroadcast;
            _wsServer.LoadUsersList += HandleLoadUsersList;
            _userService = new UserService(databaseController);
            _messageService = new MessageService(databaseController);
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
            }
            else
            {
                _wsServer.RegistrationResponse(e.Connection, RegistrationResult.UserAlreadyExists);
            }
        }
        private void HandleCheckLogin(object sender, CheckLoginEventArgs e)
        {
            var result = _userService.TryLogin(e.Login, e.Password);

            _wsServer.CheckLoginResponse(e.Login, e.Password, e.Connection, e.ClientId, e.ConnectionResponse, result);

        }
        public void Start()
        {
            Console.WriteLine($"WebSocketServer: {IPAddress.Any}:{WS_PORT}");
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
            string message = $"Клиент '{e.ClientName}' {clientState}.";

            Console.WriteLine(message);
            _wsServer.Send(message);
        }
    }
}
