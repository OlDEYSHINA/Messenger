using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Common;
using Common.Network;
using Common.Network._EventArgs_;
using Newtonsoft.Json;

namespace Server
{
    class NetworkManager
    {
        private const int WS_PORT = 65000;

        private readonly WsServer _wsServer;

        public NetworkManager()
        {
            _wsServer = new WsServer(new IPEndPoint(IPAddress.Any, WS_PORT));
            _wsServer.ConnectionStateChanged += HandleConnectionStateChanged;
            _wsServer.MessageReceived += HandleMessageReceived;

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
            // _wsServer.SendTo
            var income = JsonConvert.DeserializeObject<Message>(e.Message);
            _wsServer.Send(income.Text,income.UsernameSource,income.UsernameTarget);
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
