using Prism.Commands;
using Prism.Mvvm;
using System.Threading;
using System.Windows;
using Client.BLL.Interfaces;
using Client.BLL;

namespace Client.ViewModels
{
    class MainWindowViewModel : BindableBase
    {

        object _currentContentVM;
        public object CurrentContentVM
        {
            get
            {
                return _currentContentVM;
            }
            set
            {
                SetProperty(ref _currentContentVM, value);
            }
        }
        private string _outputView;
        public string OutputView
        {
            get
            {
                return _outputView;
            }
            set
            {
                SetProperty(ref _outputView, value);
            }
        }
        /// <summary>
        /// Выбор отображения в главном окне
        /// </summary>
        public enum ViewType
        {
            Login,
            Registration,
            Chat
        }

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
        ChatViewModel chatViewModel;
        RegistrationViewModel registrationViewModel;
        LoginVM loginVM;
        INetworkManager networkManager;

        public MainWindowViewModel()
        {
            networkManager = new NetworkManager();
            loginVM = new LoginVM(this, networkManager);
            registrationViewModel = new RegistrationViewModel(this,networkManager);
            chatViewModel = new ChatViewModel(this,networkManager);
            ChangeView(ViewType.Login);
        }
    }
}
