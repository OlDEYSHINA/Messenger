using Client.BLL;
using Client.Models;
using Common.Network;
using Common.Network._EventArgs_;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Windows;

namespace Client.ViewModels
{
    class LoginVM : BindableBase
    {
        private string _username;
        private string _password;

        private bool _correctUsername;
        private bool _correctPassword;
        private bool _enableLoginView;

        private string _errorLabel;
        private string _serverAddress;
        private string _serverPort;

        private Visibility _visibility;
        private ITransport _transport;
        private ILoginModel _login;
        MainWindowViewModel _mainWindowViewModel;

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
        public bool EnableLoginView
        {
            get
            {
                return _enableLoginView;
            }
            set
            {
                SetProperty(ref _enableLoginView, value);
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

        public string ServerAddress
        {
            get
            {
                return _serverAddress;
            }
            set
            {
                SetProperty(ref _serverAddress, value);
            }
        }
        public string ServerPort
        {
            get
            {
                return _serverPort;
            }
            set
            {
                SetProperty(ref _serverPort, value);
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
        public DelegateCommand Registration { get; }
        public DelegateCommand StartConnection { get; }

        public LoginVM(MainWindowViewModel mainWindowViewModel, ITransport transport)
        {
            _transport = transport;
            _mainWindowViewModel = mainWindowViewModel;
            _login = new LoginModel();
            SendCommand = new DelegateCommand(ConfirmLogin, () => true);
            Registration = new DelegateCommand(ShowRegistrationView, () => true);
            StartConnection = new DelegateCommand(HandleButtonStartConnectionClick, () => true);
            _correctPassword = false;
            _correctUsername = false;
            _enableLoginView = false;
#if DEBUG
            ServerAddress = "127.0.0.1";
            ServerPort = "65000";
#endif
        }
        private void HandleButtonStartConnectionClick()
        {
            try
            {
            
            _transport.ConnectionStateChanged += HandleConnectionStateChanged;
            // _currentTransport.MessageReceived += HandleMessageReceived;
            _transport.Connect(_serverAddress, _serverPort);
            }
            catch (Exception ex)
            {
                ErrorLabel=ex.Message;
               // SetDefaultButtonState();
            }
        }
        public void LoginRequest(object sender, LoginRequestEventArgs e)
        {
            if (e.Result == "Confirm")
            {

            }
            else
            {
                ErrorLabel = e.Result;
            }
        }
        public void ConfirmLogin()
        {

            if (_correctUsername & _correctPassword)
            {
                _login.Username = _username;
                _login.Password = _password;
                
                _transport?.Login(_login.Username);
            }
            else
            {
                // Вывод ошибки в окно
            }
        }

        private void HandleConnectionStateChanged(object sender, ConnectionStateChangedEventArgs e)
        {
            if (e.Connected)
            {
                if (string.IsNullOrEmpty(e.ClientName))
                {
                    EnableLoginView = true;
                    ErrorLabel = "Авторизуйтеся, чтобы отправлять сообщения.";
                }
                else
                {
                    ErrorLabel = "Авторизация успешна.";
                    _mainWindowViewModel.ChangeView(MainWindowViewModel.ViewType.Chat);
                }
            }
            else
            {
                EnableLoginView = false;
                ErrorLabel="Клиент отключен от сервера.";
               // SetDefaultButtonState();
            }
        }

        public void ShowRegistrationView()
        {
            _mainWindowViewModel.ChangeView(MainWindowViewModel.ViewType.Registration);
        }
    }
}
