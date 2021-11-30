using Common.Network._Enums_;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Network._EventArgs_
{
    public class LoginResponseReceivedEventArgs
    {
        #region Properties

        public LoginResult LoginResult { get; set; }

        #endregion Properties

        #region Constructors

        public LoginResponseReceivedEventArgs(LoginResult result)
        {
            LoginResult = result;
        }

        #endregion Constructors
    }
}
