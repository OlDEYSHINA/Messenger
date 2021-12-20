namespace Client.ViewModels
{
    using System.Windows;
    using System.Windows.Media;

    using Common.Network;

    using Models;

    using Prism.Commands;
    using Prism.Mvvm;

    internal class RegistrationViewModel : BindableBase
    {
        #region Fields

        private Visibility _loginToolTipVisibility;
        private Visibility _passwordToolTipVisibility;
        private Visibility _confirmPasswordToolTipVisibility;
        private Brush _loginColor = Brushes.Gray;

        private Brush _passwordColor = Brushes.Gray;

        private Brush _confirmPasswordColor = Brushes.Gray;
        private string _loginToolTipText;

        private string _passwordToolTipText;

        private string _confirmPasswordToolTipText;
        private readonly ITransport _transport;
        private bool _usernameIsGood;
        private bool _passwordIsGood;
        private bool _confirmPasswordIsGood;
        private string _confirmPassword;
        private string _registrationResultLabel;
        private readonly MainWindowViewModel _mainWindowViewModel;
        private readonly RegistrationModel _registrationModel;

        #endregion

        #region Properties

        public Visibility LoginToolTipVisibility
        {
            get => _loginToolTipVisibility;
            set => SetProperty(ref _loginToolTipVisibility, value);
        }

        public Visibility PasswordToolTipVisibility
        {
            get => _passwordToolTipVisibility;
            set => SetProperty(ref _passwordToolTipVisibility, value);
        }

        public Visibility ConfirmPasswordToolTipVisibility
        {
            get => _confirmPasswordToolTipVisibility;
            set => SetProperty(ref _confirmPasswordToolTipVisibility, value);
        }

        public Brush LoginColor
        {
            get => _loginColor;
            set => SetProperty(ref _loginColor, value);
        }

        public Brush PasswordColor
        {
            get => _passwordColor;
            set => SetProperty(ref _passwordColor, value);
        }

        public Brush ConfirmPasswordColor
        {
            get => _confirmPasswordColor;
            set => SetProperty(ref _confirmPasswordColor, value);
        }

        public string LoginToolTipText
        {
            get => _loginToolTipText;
            set => SetProperty(ref _loginToolTipText, value);
        }

        public string PasswordToolTipText
        {
            get => _passwordToolTipText;
            set => SetProperty(ref _passwordToolTipText, value);
        }

        public string ConfirmPasswordToolTipText
        {
            get => _confirmPasswordToolTipText;
            set => SetProperty(ref _confirmPasswordToolTipText, value);
        }

        public string Username
        {
            get => _registrationModel.Username;
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    LoginToolTipText = "Необходимо заполнить поле Логин";
                    LoginColor = Brushes.Red;
                    LoginToolTipVisibility = Visibility.Visible;
                    _usernameIsGood = false;
                }
                else if (value.Length < 3)
                {
                    {
                        LoginToolTipText = "Логин не должен быть короче 3 символов";
                        LoginColor = Brushes.Red;
                        LoginToolTipVisibility = Visibility.Visible;
                        _usernameIsGood = false;
                    }
                }
                else if (value.Length > 30)
                {
                    LoginToolTipText = "Логин не должен быть длиннее 30 символов";
                    LoginColor = Brushes.Red;
                    LoginToolTipVisibility = Visibility.Visible;
                    _usernameIsGood = false;
                }
                else
                {
                    LoginToolTipVisibility = Visibility.Collapsed;
                    LoginColor = Brushes.Gray;
                    _registrationModel.Username = value;
                    _usernameIsGood = true;
                }
            }
        }

        public string Password
        {
            get => _registrationModel.Password;
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    PasswordToolTipText = "Необходимо заполнить поле Пароль";
                    PasswordColor = Brushes.Red;
                    PasswordToolTipVisibility = Visibility.Visible;
                    _passwordIsGood = false;
                }
                else if (value.Length < 3)
                {
                    PasswordToolTipText = "Пароль не может быть короче 3 символов";
                    PasswordColor = Brushes.Red;
                    PasswordToolTipVisibility = Visibility.Visible;
                    _passwordIsGood = false;
                }
                else if (value.Length > 30)
                {
                    PasswordToolTipText = "Пароль не может быть длиннее 30 символов";
                    PasswordColor = Brushes.Red;
                    PasswordToolTipVisibility = Visibility.Visible;
                    _passwordIsGood = false;
                }
                else
                {
                    PasswordToolTipVisibility = Visibility.Collapsed;
                    PasswordColor = Brushes.Gray;
                    _registrationModel.Password = value;
                    _passwordIsGood = true;
                }
            }
        }

        public string ConfirmPassword
        {
            get => _confirmPassword;
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    ConfirmPasswordToolTipText = "Необходимо заполнить поле Подтверждение пароля";
                    ConfirmPasswordColor = Brushes.Red;
                    ConfirmPasswordToolTipVisibility = Visibility.Visible;
                    _confirmPasswordIsGood = false;
                }
                else if (Password != value)
                {
                    ConfirmPasswordToolTipText = "Пароли должны совпадать";
                    ConfirmPasswordColor = Brushes.Red;
                    ConfirmPasswordToolTipVisibility = Visibility.Visible;
                    _confirmPasswordIsGood = false;
                }
                else
                {
                    ConfirmPasswordToolTipVisibility = Visibility.Collapsed;
                    ConfirmPasswordColor = Brushes.Gray;
                    SetProperty(ref _confirmPassword, value);
                    _confirmPasswordIsGood = true;
                }
            }
        }

        public string RegistrationResultLabel
        {
            get => _registrationResultLabel;
            set => SetProperty(ref _registrationResultLabel, value);
        }

        public DelegateCommand ConfirmRegistration { get; }

        public DelegateCommand CancelRegistrationCommand { get; }

        #endregion

        #region Constructors

        public RegistrationViewModel(MainWindowViewModel mainWindowViewModel, ITransport transport)
        {
            _transport = transport;
            _transport.RegistrationResponseReceived += HandleRegistrationResponseReceived;
            _loginToolTipVisibility = Visibility.Collapsed;
            _passwordToolTipVisibility = Visibility.Collapsed;
            _confirmPasswordToolTipVisibility = Visibility.Collapsed;
            _usernameIsGood = false;
            _passwordIsGood = false;
            _confirmPasswordIsGood = false;
            _mainWindowViewModel = mainWindowViewModel;
            ConfirmRegistration = new DelegateCommand(ConfirmRegistrationAction, () => true);
            CancelRegistrationCommand = new DelegateCommand(CancelRegistration, () => true);
            _registrationModel = new RegistrationModel();
        }

        #endregion

        #region Methods

        public void ConfirmRegistrationAction()
        {
            if (_checkAllValidate())
            {
                _transport.Registration(Username, Password);
            }
        }

        public void CancelRegistration()
        {
            _mainWindowViewModel.ChangeView(MainWindowViewModel.ViewType.Login);
        }

        private bool _checkAllValidate()
        {
            bool allOk = true;

            if (!_usernameIsGood)
            {
                Username = _registrationModel.Username;
                allOk = false;
            }

            if (!_passwordIsGood)
            {
                Password = _registrationModel.Password;
                allOk = false;
            }

            if (!_confirmPasswordIsGood)
            {
                ConfirmPassword = _confirmPassword;
                allOk = false;
            }

            if (allOk)
            {
                return true;
            }

            return false;
        }

        private void HandleRegistrationResponseReceived(object sender, RegistrationResponseReceivedEventArgs e)
        {
            if (e.RegistrationResult == RegistrationResult.Ok)
            {
                _mainWindowViewModel.ChangeView(MainWindowViewModel.ViewType.Login);
            }
            else if (e.RegistrationResult == RegistrationResult.UserAlreadyExists)
            {
                RegistrationResultLabel = "Пользователь уже существует";
            }
            else
            {
                RegistrationResultLabel = "Неизвестная ошибка";
            }
        }

        #endregion
    }
}
