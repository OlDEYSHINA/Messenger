using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebSocketSharp;
using WebSocketSharp.Server;
using Newtonsoft.Json;
using Common;

namespace Server
{
    class Program
    {
        static WebSocketServer socketServer;
        static void Main(string[] args)
        {
            
            socketServer = new WebSocketServer("ws://127.0.0.1:4649");

            socketServer.AddWebSocketService<MainMessageHandler>("/Chat");
            socketServer.AddWebSocketService<Registration>("/Registration"); // Смотри сюда
            socketServer.AddWebSocketService<Login>("/Login");
            
            socketServer.Start();

            if (socketServer.IsListening)
            {
                Console.WriteLine($"Listening on port {socketServer.Port}, and providing WebSocket services:");

                foreach (var path in socketServer.WebSocketServices.Paths) Console.WriteLine($"- {path}");
            }
           
            Console.WriteLine("Servers started. Press the key to exit");
            Console.ReadKey(true);
        }
        public class Login : WebSocketBehavior
        {
            protected override void OnMessage(MessageEventArgs e)
            {
                var loginData = JsonConvert.DeserializeObject<Common.LoginMessage>(e.Data);
                Console.WriteLine("Имя пользователя - "+loginData.Username+" Пароль "+loginData.Password+" id "+ base.ID);
                if(loginData.Username=="Admin"&& loginData.Password == "Admin")
                {
                    Sessions.SendTo("Confirm", base.ID);
                }
                else
                {
                    Sessions.SendTo("Unknown user",base.ID);
                }
                
            }
        }
        public class Registration : WebSocketBehavior
        {
            protected override void OnMessage(MessageEventArgs e)
            {
                base.OnMessage(e);
            }
        }
        internal class MainMessageHandler : WebSocketBehavior
        {
                       
            protected override void OnClose(CloseEventArgs e)
            {
                base.OnClose(e);
                
                Console.WriteLine($"{e}The connection was closed.");
            }

            protected override void OnError(ErrorEventArgs e)
            {
                base.OnError(e);

                Console.WriteLine($"Server error. Message: {e.Message}");
            }

            protected override void OnOpen()
            {
                base.OnOpen();

                Console.WriteLine("The connection to main server was open.");
            }

            protected override void OnMessage(MessageEventArgs args)
            {
                Sessions.Broadcast("1 "+args.Data+" АЙЛДИ "+ Sessions.ActiveIDs);
                //var it = Sessions.ActiveIDs;
                

                //if (string.IsNullOrEmpty(args.Data))
                //    return;
                //Console.WriteLine($"Message: {args.Data}");
                //Send($"{args.Data} : Server Ok");
            }
        }
    }
}
