using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WebSocketSharp;

namespace Client
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        WebSocket client = new WebSocket("ws://127.0.0.1:60111/test");
        public MainWindow()
        {

            InitializeComponent();
           
            client.OnMessage += Client_OnMessage;
            client.Connect();

            Console.WriteLine("Enter message:");

           /* while (true)
            {
                client.Send(Console.ReadLine());
            }*/

        }
        private static void Client_OnMessage(object sender, MessageEventArgs e)
        {
            Console.WriteLine(e.Data);
            Console.WriteLine("Enter message:");
        }

        private void SendMessageButton_Click(object sender, RoutedEventArgs e)
        {
            client.Send(MessageTextBox.Text);
        }
    }
}
