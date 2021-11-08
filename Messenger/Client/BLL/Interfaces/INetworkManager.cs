using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.BLL.Interfaces
{
    public interface INetworkManager
    {
        event EventHandler<ChatMessageEventArgs> MessageRecieved;
        event EventHandler<LoginRequestEventArgs> LoginRequestRecieved;
        void StartConnection();
        void SendMessage(string message);
        void SendLogin(object data);

    }

}
