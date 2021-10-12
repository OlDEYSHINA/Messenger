using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebSocketSharp;

namespace Client.BLL
{

    class ConnectManager
    {
      
       public void StartConnection()
        {
            var client = new WebSocket("ws://127.0.0.1:60111/test");
            client.Connect();
            client.OnOpen += (sender, e) =>
              {

              };
        }

        
        

          

    }
}
