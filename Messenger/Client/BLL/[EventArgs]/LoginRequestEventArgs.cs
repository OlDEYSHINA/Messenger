using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.BLL
{
    public class LoginRequestEventArgs
    {
        public string Result { get; set; }
        public LoginRequestEventArgs(string result)
        {
            
            Result = result;
        }
    }
}
