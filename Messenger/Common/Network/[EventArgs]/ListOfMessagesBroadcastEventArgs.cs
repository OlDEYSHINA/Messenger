using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Network._EventArgs_
{
    public class ListOfMessagesBroadcastEventArgs
    {
        #region Properties

        public string MyLogin { get; set; }
        public string CompanionLogin { get; set; }
        public WsConnection Connection { get; set; }

        #endregion Properties

        #region Constructors

        public ListOfMessagesBroadcastEventArgs(WsConnection connection,string myLogin,string companionLogin)
        {
            Connection = connection;
            MyLogin = myLogin;
            CompanionLogin = companionLogin;
        }

        #endregion Constructors
    }
}
