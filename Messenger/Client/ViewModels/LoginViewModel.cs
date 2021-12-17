namespace Client.ViewModels
{
    using System;
    using System.Windows;

    using Common.Network;

    using Models;

    using Prism.Commands;
    using Prism.Mvvm;

    internal class LoginVM : BindableBase
    {
        #region Fields

        private string _username;
        private string _password;

        private bool _correctUsername;
        private bool _correctPassword;
        private bool _enableLoginView;
        private bool _enableConnectionButton;

        private string _errorLabel;
        private string _serverAddress;
        private string _serverPort;

        private Visibility _visibility;
        private readonly ITransport _transport;
        private readonly LoginModel _login;
        private readonly MainWindowViewModel _mainWindowViewModel;

        #endregion

        #region Properties

        public Visibility Visibility
        {
            get => _visibility;
            set => SetProperty(ref _visibility, value);
        }

        public bool EnableLoginView
        {
            get => _enableLoginView;
            set => SetProperty(ref _enableLoginView, value);
        }

        public bool EnableConnectionButton
        {
            get => _enableConnectionButton;
            set => SetProperty(ref _enableConnectionButton, value);
        }

        public string ErrorLabel
        {
            get => _errorLabel;
            set => SetProperty(ref _errorLabel, value);
        }

        public string ServerAddress
        {
            get => _serverAddress;
            set => SetProperty(ref _serverAddress, value);
        }

        public string ServerPort
        {
            get => _serverPort;
            set => SetProperty(ref _serverPort, value);
        }

        public string UsernameLogin
        {
            get => _username;
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
            get => _password;
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

        #endregion

        #region Constructors

        public LoginVM(MainWindowViewModel mainWindowViewModel, ITransport transport)
        {
            _transport = transport;
            _mainWindowViewModel = mainWindowViewModel;
            _transport.LoginResponseReceived += HandleLoginReceived;
            _login = new LoginModel();
            SendCommand = new DelegateCommand(ConfirmLogin, () => true);
            Registration = new DelegateCommand(ShowRegistrationView, () => true);
            StartConnection = new DelegateCommand(HandleButtonStartConnectionClick, () => true);
            _correctPassword = false;
            _correctUsername = false;
            _enableLoginView = false;
            EnableConnectionButton = true;
#if DEBUG
            ServerAddress = "127.0.0.1";
            ServerPort = "65000";
#endif
        }

        #endregion

        #region Methods

        public void ConfirmLogin()
        {
            if (_correctUsername & _correctPassword)
            {
                _login.Username = _username;
                _login.Password = _password;
                EnableLoginView = false;
                _transport?.Login(_login.Username, _login.Password);
            }
            else if (UsernameLogin != null && PasswordLogin != null)
            {
                UsernameLogin = UsernameLogin;
                PasswordLogin = PasswordLogin;
            }
        }

        public void ShowRegistrationView()
        {
            _mainWindowViewModel.ChangeView(MainWindowViewModel.ViewType.Registration);
        }

        private void HandleButtonStartConnectionClick()
        {
            try
            {
                ErrorLabel = "Попытка подключиться к серверу";
                EnableConnectionButton = false;
                _transport.ConnectionStateChanged += HandleConnectionStateChanged;
                _transport.Connect(_serverAddress, _serverPort);
            }
            catch (Exception ex)
            {
                EnableConnectionButton = true;
                ErrorLabel = ex.Message;
            }
        }

        private void HandleLoginReceived(object sender, LoginResponseReceivedEventArgs e)
        {
            if (e.LoginResult == LoginResult.Ok)
            {
                _mainWindowViewModel.ChangeView(MainWindowViewModel.ViewType.Chat);
            }
            else if (e.LoginResult == LoginResult.UnknownUser)
            {
                ErrorLabel = "Неизвестный пользователь";
            }
            else if (e.LoginResult == LoginResult.UnknownPassword)
            {
                ErrorLabel = "Неправильный пароль";
            }
            else
            {
                ErrorLabel = "Неизвестная ошибка";
            }
        }

        private void HandleConnectionStateChanged(object sender, ConnectionStateChangedEventArgs e)
        {
            EnableConnectionButton = true;
            ErrorLabel = e.Reason;

            if (e.Connected)
            {
                if (string.IsNullOrEmpty(e.ClientName))
                {
                    EnableLoginView = true;
                }
                else if (e.Reason == null)
                {
                    _mainWindowViewModel.ChangeView(MainWindowViewModel.ViewType.Chat);
                }
                else
                {
                    EnableLoginView = true;
                }
            }
            else
            {
                EnableLoginView = false;
                _mainWindowViewModel.ChangeView(MainWindowViewModel.ViewType.Login);
            }
        }

        #endregion
    }
}
