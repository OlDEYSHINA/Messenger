
using Client.BLL;
using Client.Models;
using Prism.Commands;
using Prism.Mvvm;
using System.Windows;
using System.Windows.Media;
using Common.Network;

namespace Client.ViewModels
{

    class RegistrationViewModel : BindableBase
    {
        
        #region Visibility

        private Visibility _loginToolTipVisibility;
        private Visibility _passwordToolTipVisibility;
        private Visibility _confirmPasswordToolTipVisibility;
        private Visibility _eMailToolTipVisibility;

        public Visibility LoginToolTipVisibility
        {
            get
            {
                return _loginToolTipVisibility;
            }
            set
            {
                SetProperty(ref _loginToolTipVisibility, value);
            }
        }
        public Visibility PasswordToolTipVisibility
        {
            get
            {
                return _passwordToolTipVisibility;
            }
            set
            {
                SetProperty(ref _passwordToolTipVisibility, value);
            }
        }
        public Visibility ConfirmPasswordToolTipVisibility
        {
            get
            {
                return _confirmPasswordToolTipVisibility;
            }
            set
            {
                SetProperty(ref _confirmPasswordToolTipVisibility, value);
            }
        }
        public Visibility EMailToolTipVisibility
        {
            get
            {
                return _eMailToolTipVisibility;
            }
            set
            {
                SetProperty(ref _eMailToolTipVisibility, value);
            }
        }
        #endregion //Visibility

        #region TextBoxBorderColor
        private Brush _loginColor = Brushes.Gray;
        public Brush LoginColor
        {
            get
            {
                return _loginColor;
            }
            set
            {
                SetProperty(ref _loginColor, value);
            }
        }

        private Brush _passwordColor = Brushes.Gray;
        public Brush PasswordColor
        {
            get
            {
                return _passwordColor;
            }
            set
            {
                SetProperty(ref _passwordColor, value);
            }
        }

        private Brush _confirmPasswordColor = Brushes.Gray;
        public Brush ConfirmPasswordColor
        {
            get
            {
                return _confirmPasswordColor;
            }
            set
            {
                SetProperty(ref _confirmPasswordColor, value);
            }
        }

        private Brush _eMailColor = Brushes.Gray;
        public Brush EMailColor
        {
            get
            {
                return _eMailColor;
            }
            set
            {
                SetProperty(ref _eMailColor, value);
            }
        }
        #endregion // TextBoxBorderColor

        #region ToolTipText
        private string _loginToolTipText;
        public string LoginToolTipText
        {
            get
            {
                return _loginToolTipText;
            }
            set
            {
                SetProperty(ref _loginToolTipText, value);
            }
        }

        private string _passwordToolTipText;
        public string PasswordToolTipText
        {
            get
            {
                return _passwordToolTipText;
            }
            set
            {
                SetProperty(ref _passwordToolTipText, value);
            }
        }

        private string _confirmPasswordToolTipText;
        public string ConfirmPasswordToolTipText
        {
            get
            {
                return _confirmPasswordToolTipText;
            }
            set
            {
                SetProperty(ref _confirmPasswordToolTipText, value);
            }
        }

        private string _eMailToolTipText;
        public string EMailToolTipText
        {
            get
            {
                return _eMailToolTipText;
            }
            set
            {
                SetProperty(ref _eMailToolTipText, value);
            }
        }
        #endregion //ToolTipText
        private ITransport _transport;
        private bool _usernameIsGood;
        private bool _passwordIsGood;
        private bool _confirmPasswordIsGood;
        private bool _eMailIsGood;
        private string _confirmPassword;
        MainWindowViewModel _mainWindowViewModel;
        IRegistrationModel _registrationModel;
        public string Username
        {
            get
            {
                return _registrationModel.Username;
            }
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    LoginToolTipText = "Необходимо заполнить поле Логин";
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
            get
            {
                return _registrationModel.Password;
            }
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    PasswordToolTipText = "Необходимо заполнить поле Пароль";
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
            get
            {
                return _confirmPassword;
            }
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    ConfirmPasswordToolTipText = "Необходимо заполнить поле Подтверждение пароля";
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

        public string EMail
        {
            get
            {
                return _registrationModel.EMail;
            }
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    EMailToolTipText = "Необходимо заполнить поле Электронная почта";
                    EMailColor = Brushes.Red;
                    EMailToolTipVisibility = Visibility.Visible;
                    _eMailIsGood = false;
                }
                else
                {
                    EMailToolTipVisibility = Visibility.Collapsed;
                    EMailColor = Brushes.Gray;
                    _registrationModel.EMail = value;
                    _eMailIsGood = true;
                }
            }
        }
        
        public DelegateCommand ConfirmRegistration { get; }

        public RegistrationViewModel(MainWindowViewModel mainWindowViewModel, ITransport transport)
        {
            _transport = transport;
            _loginToolTipVisibility = Visibility.Collapsed;
            _passwordToolTipVisibility = Visibility.Collapsed;
            _confirmPasswordToolTipVisibility = Visibility.Collapsed;
            _eMailToolTipVisibility = Visibility.Collapsed;
            _usernameIsGood = false;
            _passwordIsGood = false;
            _confirmPasswordIsGood = false;
            _eMailIsGood = false;
            _mainWindowViewModel = mainWindowViewModel;
            ConfirmRegistration = new DelegateCommand(ConfirmRegistrationAction, () => true);
            _registrationModel = new RegistrationModel();
        }

        public void ConfirmRegistrationAction()
        {
            if (_checkAllValidate())
            {
                _mainWindowViewModel.ChangeView(MainWindowViewModel.ViewType.Login);
            }
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
            if (!_eMailIsGood)
            {
                EMail = _registrationModel.EMail;
                allOk = false;
            }
            if (allOk)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
