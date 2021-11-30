using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Network._EventArgs_
{
    public class RegistrationRequestEventArgs
    {
        #region Properties

        public string Login { get; set; }
        public string Password { get; set; }
        public WsConnection Connection { get; set; }

        #endregion Properties

        #region Constructors

        public RegistrationRequestEventArgs(string login,string password,WsConnection connection)
        {
            Login = login;
            Password = password;
            Connection = connection;
        }

        #endregion Constructors

    }
}
