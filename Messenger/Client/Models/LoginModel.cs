using System.ComponentModel;
using System.Runtime.CompilerServices;
using Common;


namespace Client.Models
{
    class LoginModel : ILoginModel
    {
        private string _username;
        private string _password;

        public string Username
        {
            get
            {
                return _username;
            }
            set
            {
                _username = value;
            }
        }
        public string Password
        {
            get
            {
                return _password;
            }
            set
            {
                _password = value;
            }
        }
    }
}
