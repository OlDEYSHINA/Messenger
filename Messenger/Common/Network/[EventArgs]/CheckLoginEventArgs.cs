using System;
using Common.Network.Messages;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Network._EventArgs_
{
    public class CheckLoginEventArgs
    {
        #region Properties

        public string Login { get; set; }
        public string Password { get; set; }
        public WsConnection Connection { get; set; }
        public Guid ClientId { get; set; }
        public ConnectionResponse ConnectionResponse { get; set; }

        #endregion Properties

        #region Constructors

        public CheckLoginEventArgs(string login, string password,WsConnection connection,
            Guid clientId,ConnectionResponse connectionResponse)
        {
            Login = login;
            Password = password;
            Connection = connection;
            ClientId = clientId;
            ConnectionResponse = connectionResponse;
        }

        #endregion Constructors
    }
}
