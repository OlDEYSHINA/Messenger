using Client.BLL;
using Client.BLL.Interfaces;
using Client.Models;
using Prism.Commands;
using Prism.Mvvm;
using System.Windows;


namespace Client.ViewModels
{
    class LoginVM : BindableBase
    {
        private string _username;
        private bool _correctUsername;

        private string _password;
        private bool _correctPassword;

        private string _errorLabel;
        private Visibility _visibility;

        private ILoginModel _login;
        private INetworkManager _networkManager;

        public Visibility Visibility
        {
            get
            {
                return _visibility;
            }
            set
            {
                SetProperty(ref _visibility, value);
            }
        }

        public string ErrorLabel
        {
            get
            {
                return _errorLabel;
            }
            set
            {
                SetProperty(ref _errorLabel, value);
            }
        }

        public string UsernameLogin
        {
            get
            {
                return _username;
            }
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    _correctUsername = false;
                    ErrorLabel = "Заполните поле имя пользователя";
                }
                else
                {
                    
                    SetProperty(ref _username, value);
                    _correctUsername = true;
                }
            }
        }

        public string PasswordLogin
        {
            get
            {
                return _password;
            }
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    _correctPassword = false;
                    ErrorLabel = "Заполните поле пароль";
                }
                else
                {
                    SetProperty(ref _password, value);
                    _correctPassword = true;
                }
            }
        }
        public DelegateCommand SendCommand { get; }
        MainWindowViewModel _mainWindowViewModel;
        public void ConfirmLogin()
        {
            if (_correctUsername & _correctPassword)
            {
                MessageBox.Show("Luck");

                _networkManager.StartConnection();
                _mainWindowViewModel.ChangeView(MainWindowViewModel.ViewType.Chat);
             //   Visibility = Visibility.Hidden;
            }
            else
            {
                // Вывод ошибки в окно
            }
        }

        public LoginVM(MainWindowViewModel mainWindowViewModel)
        {
            _mainWindowViewModel = mainWindowViewModel;
            _login = new LoginModel { Username = "Amogus", Password = "Aboba" };
            SendCommand = new DelegateCommand(ConfirmLogin, () => true);
            _networkManager = new NetworkManager();
            _correctPassword = false;
            _correctUsername = false;

        }
      
    }
}
