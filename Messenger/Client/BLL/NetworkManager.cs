using Client.Models;
using System.Windows;

using Client.ViewModels;
using System;
using Newtonsoft.Json;

namespace Client.BLL
{
    /*
    public class NetworkManager : INetworkManager
    {
        public WebSocket client;
        public WebSocket loginClient;
        public bool ConnectionIsOk;
        public event EventHandler<ChatMessageEventArgs> MessageRecieved;
        public event EventHandler<LoginRequestEventArgs> LoginRequestRecieved;
        public string Channel;

        public NetworkManager()
        {
          //  _loginModel = new LoginModel();
        }

        public void StartLoginConnection()                          // Область Входа в программу ( передача и получение логина )
        {
            loginClient = new WebSocket("ws://127.0.0.1:65000/Login");
            loginClient.OnMessage += HandleLoginRequest;
            loginClient.Connect();
        }
        
        public void SendLogin(object data)
        {
            var message = JsonConvert.SerializeObject(data);
            if (client == null)
            {
                StartLoginConnection();
            }
            loginClient.Send(message);
        }
        private void HandleLoginRequest(object sender, MessageEventArgs e)
        {
            LoginRequestRecieved.Invoke(this, new LoginRequestEventArgs(e.Data));
        }

        public void StartConnection()
        {
            client = new WebSocket("ws://127.0.0.1:4649/Chat");
            client.OnOpen += (sender, e) =>
            {

            };
            client.OnMessage += HandleMessage;
            client.Connect();
            // var state = client.ReadyState;
        }

        private void HandleMessage(object sender, MessageEventArgs e)
        {
            MessageRecieved.Invoke(this,new ChatMessageEventArgs("/Chat","To","from",e.Data,DateTime.Now));
        }
     
        public void SendMessage(string message)
        {
            
            if (client == null)
            {
                StartConnection();
            }
            if (client.ReadyState != WebSocketState.Open)
            {
                var aboba = client;
            }
            client.Send(message);
        }

        void onOpen(object sender, System.EventArgs e)
        {
            //  MessageBox.Show("booba");
        }

    }*/

}
