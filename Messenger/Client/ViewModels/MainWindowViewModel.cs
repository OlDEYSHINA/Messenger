namespace Client.ViewModels
{
    using System.Windows;

    using Common.Network;

    using Prism.Mvvm;

    internal class MainWindowViewModel : BindableBase
    {
        #region Enums

        /// <summary>
        /// Выбор отображения в главном окне
        /// </summary>
        public enum ViewType
        {
            Login,
            Registration,
            Chat
        }

        #endregion

        #region Fields

        private ChatViewModel chatViewModel;
        private readonly RegistrationViewModel registrationViewModel;
        private readonly LoginVM loginVM;
        private ITransport _transport;
        private object _currentContentVM;
        private string _outputView;

        #endregion

        #region Properties

        public object CurrentContentVM
        {
            get => _currentContentVM;
            set => SetProperty(ref _currentContentVM, value);
        }

        public string OutputView
        {
            get => _outputView;
            set => SetProperty(ref _outputView, value);
        }

        #endregion

        #region Constructors

        public MainWindowViewModel()
        {
            _transport = new WsClient();
            loginVM = new LoginVM(this, _transport);
            registrationViewModel = new RegistrationViewModel(this, _transport);

            ChangeView(ViewType.Login);
            Application.Current.Exit += Current_Exit;
        }

        #endregion

        #region Methods

        public void ChangeView(ViewType viewType)
        {
            switch (viewType)
            {
                case ViewType.Login:
                {
                    CurrentContentVM = loginVM;

                    break;
                }

                case ViewType.Chat:
                {
                    chatViewModel = new ChatViewModel(this, _transport, loginVM.UsernameLogin);
                    CurrentContentVM = chatViewModel;

                    break;
                }

                case ViewType.Registration:
                {
                    CurrentContentVM = registrationViewModel;

                    break;
                }
            }
        }

        public void ReloadWsClient()
        {
            _transport = null;
            _transport = new WsClient();
        }

        private void Current_Exit(object sender, ExitEventArgs e)
        {
            _transport.Disconnect();
        }

        #endregion
    }
}
