using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Threading;
using System.Windows;

using Common.Network;
using System.ComponentModel;

namespace Client.ViewModels
{
    class MainWindowViewModel : BindableBase
    {
       
        ChatViewModel chatViewModel;
        RegistrationViewModel registrationViewModel;
        LoginVM loginVM;
        private ITransport _transport;
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
                        
                        chatViewModel = new ChatViewModel(this, _transport,loginVM.UsernameLogin);
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
       
        public MainWindowViewModel()
        {
            _transport = new WsClient();
            loginVM = new LoginVM(this, _transport);
            registrationViewModel = new RegistrationViewModel(this, _transport);
            
            ChangeView(ViewType.Login);
            Application.Current.Exit += Current_Exit;
        }

        private void Current_Exit(object sender, ExitEventArgs e)
        {
            _transport.Disconnect();
        }
    }
}
