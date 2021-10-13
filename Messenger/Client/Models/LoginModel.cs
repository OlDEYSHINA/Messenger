using System.ComponentModel;
using System.Runtime.CompilerServices;


namespace Client.Models
{
    class LoginModel : INotifyPropertyChanged
    {
        private string username;
        private string password;
        public string Username
        {
            get
            {
                return username;
            }
            set
            {
                username = value;
                OnPropertyChanged("UsernameLogin");
            }
        }
        public string Password
        {
            get
            {
                return password;
            }
            set
            {
                if (string.IsNullOrEmpty(value))
                {

                }
                else
                {
                    password = value;
                    OnPropertyChanged("PasswordLogin");
                }
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));

        }
    }
}
