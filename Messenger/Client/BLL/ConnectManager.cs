using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Threading.Tasks;
using WebSocketSharp;
using System.Threading;

namespace Client.BLL
{

    class ConnectManager
    {
      
      static public void StartConnection()
        {
          //  bool connetionIsGood = false;
            var client = new WebSocket("ws://127.0.0.1:60111/test");
            client.OnOpen += onOpen;
            client.ConnectAsync();
               
            /*
            if (!connetionIsGood)
            {
                MessageBox.Show("Нет ответа от сервера");
                System.Environment.Exit(0);
               // System.Environment.FailFast("NonConnection");
            };*/



        }

        static void onOpen(object sender,System.EventArgs e)
        {
            MessageBox.Show("Есть немного ответов, совсем чуть-чуть");
           // connetionIsGood = true;
            var loginWindow = new LoginWindow();
            loginWindow.Show();
        }


        
        

          

    }
}
