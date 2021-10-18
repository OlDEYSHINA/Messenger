using Client.BLL.Interfaces;
using Client.Models;
using WebSocketSharp;


namespace Client.BLL
{

    public class NetworkManager : INetworkManager
    {
        private ILoginModel _loginModel;
        public void StartConnection()
        {
            var client = new WebSocket("ws://127.0.0.1:60111/test");
            client.OnOpen += onOpen;
            client.ConnectAsync();
            var state = client.ReadyState;
        }

        void onOpen(object sender, System.EventArgs e)
        {
        }

        public NetworkManager()
        {
            _loginModel = new LoginModel();
        }
    }
}
