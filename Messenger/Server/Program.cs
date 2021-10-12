using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebSocketSharp;
using WebSocketSharp.Server;

namespace Server
{
    class Program
    {
        static void Main(string[] args)
        {
            var socketServer = new WebSocketServer("ws://127.0.0.1:60111");

            socketServer.AddWebSocketService<MainMessageHandler>("/test");

            socketServer.Start();


            if (socketServer.IsListening)
            {
                Console.WriteLine($"Listening on port {socketServer.Port}, and providing WebSocket services:");

                foreach (var path in socketServer.WebSocketServices.Paths) Console.WriteLine($"- {path}");
            }

            Console.WriteLine("Servers started. Press the key to exit");
            Console.ReadKey(true);
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
                if (string.IsNullOrEmpty(args.Data))
                    return;
                Console.WriteLine($"Message: {args.Data}");

                Send($"{args.Data} : Server Ok");
            }
        }
    }
}
