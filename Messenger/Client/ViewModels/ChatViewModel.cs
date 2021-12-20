namespace Client.ViewModels
{
    using System;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Windows;

    using BLL;

    using Common;
    using Common.Network;

    using Models;

    using Newtonsoft.Json;

    using Prism.Commands;
    using Prism.Mvvm;

    using Services;

    internal class ChatViewModel : BindableBase
    {
        #region Fields

        private readonly ITransport _transport;

        private string _message;
        private readonly string _myLogin;
        private bool _IsDarkTheme;

        private UserState _selectedUser;

        private readonly ChatModel _chatModel;
        private readonly ChatMenuService _chatMenuService;
        private EventLogViewModel _eventLogViewModel;
        private SettingsViewModel _settingsViewModel;

        #endregion

        #region Properties

        public bool IsDarkTheme
        {
            get => _IsDarkTheme;
            set => SetProperty(ref _IsDarkTheme, value);
        }

        public UserState SelectedUser
        {
            get => _selectedUser;
            set
            {
                SetProperty(ref _selectedUser, value);

                if (value == null)
                {
                    ChatMessages = _chatModel.GetChat("Global");
                }
                else
                {
                    ChatMessages = _chatModel.GetChat(value.Name);
                }
            }
        }

        public DelegateCommand SendMessage { get; }

        public DelegateCommand OpenEventLog { get; }

        public DelegateCommand MenuExitButton { get; }

        public DelegateCommand MenuSettingsButton { get; }

        public DelegateCommand MenuAboutButton { get; }

        public DelegateCommand<object> SendMessageToServerCommand { get; }

        public ObservableCollection<ObservableMessage> ChatMessages
        {
            get => _chatModel.CurrentChat;
            set => SetProperty(ref _chatModel.CurrentChat, value);
        }

        public ObservableCollection<UserState> UsersStatusesCollection
        {
            get => _chatModel.UserStatuses;
            set => SetProperty(ref _chatModel.UserStatuses, value);
        }

        public string Message
        {
            get => _message;
            set
            {
                if (value.Length > 300)
                {
                }
                else
                {
                    SetProperty(ref _message, value);
                }
            }
        }

        #endregion

        #region Constructors

        public ChatViewModel(MainWindowViewModel mainWindowViewModel, ITransport transport, string login)
        {
            _myLogin = login;
            _transport = transport;
            _settingsViewModel = new SettingsViewModel(this);
            _chatModel = new ChatModel(_myLogin, _transport);
            SelectedUser = new UserState("Global", true);
            _chatMenuService = new ChatMenuService();
            _transport.MessageReceived += HandleMessageReceived;
            _transport.UsersStatusesReceived += HandleUsersStatusesRequest;
            _transport.UserStateChanged += HandleUserStateChange;
            _transport.ListOfMessagesReceived += HandleListOfMessagesReceived;
            SendMessage = new DelegateCommand(SendMessageToServer, () => true);

            MenuExitButton = new DelegateCommand(_chatMenuService.Exit, () => true);
            MenuSettingsButton = new DelegateCommand(_chatMenuService.Settings, () => true);
            MenuAboutButton = new DelegateCommand(_chatMenuService.About, () => true);
            OpenEventLog = new DelegateCommand(OpenEventLogWindow, () => true);

            SendMessageToServerCommand = new DelegateCommand<object>(SendMessageToServer);
        }

        #endregion

        #region Methods

        public void OpenEventLogWindow()
        {
            _eventLogViewModel = new EventLogViewModel(_transport);
            _eventLogViewModel.OpenWindow();
        }

        public void SendMessageToServer()
        {
            if (!string.IsNullOrEmpty(Message))
            {
                var message = new Message
                              {
                                  Text = Message,
                                  UsernameSource = _myLogin,
                                  UsernameTarget = SelectedUser.Name,
                                  Time = DateTime.Now
                              };
                string serializeMessage = JsonConvert.SerializeObject(message);
                _transport?.Send(serializeMessage);
                Message = null;
            }
        }

        public void SendMessageToServer(object param)
        {
            SendMessageToServer();
        }

        private void HandleUserStateChange(object sender, UserStateChangedEventArgs e)
        {
            UserState found = UsersStatusesCollection.FirstOrDefault(x => x.Name == e.user.Name);

            if (found != null)
            {
                int foundIndex = UsersStatusesCollection.IndexOf(found);
                Application.Current.Dispatcher.Invoke(
                    () =>
                    {
                        UsersStatusesCollection.RemoveAt(foundIndex);
                        UsersStatusesCollection.Insert(foundIndex, e.user);
                        found.IsOnline = e.user.IsOnline;
                    });
            }
            else
            {
                Application.Current.Dispatcher.Invoke(() => UsersStatusesCollection.Add(e.user));
            }
        }

        private void HandleMessageReceived(object sender, MessageReceivedEventArgs e)
        {
            Message incomeMessage = null;

            try
            {
                incomeMessage = JsonConvert.DeserializeObject<Message>(e.Message);
            }
            catch
            {
                if (incomeMessage == null)
                {
                    incomeMessage = new Message
                                    {
                                        Text = e.Message,
                                        UsernameTarget = "Global",
                                        Time = DateTime.Now,
                                        UsernameSource = "server"
                                    };
                }
            }

            bool isMyMessage;

            if (incomeMessage.UsernameSource == _myLogin)
            {
                isMyMessage = true;
            }
            else
            {
                isMyMessage = false;
            }

            var observableIncome = new ObservableMessage
                                   {
                                       UsernameTarget = incomeMessage.UsernameTarget,
                                       UsernameSource = incomeMessage.UsernameSource,
                                       Time = incomeMessage.Time,
                                       IsMyMessage = isMyMessage,
                                       Text = incomeMessage.Text
                                   };

            if (incomeMessage.UsernameTarget == "Global")
            {
                if (SelectedUser.Name == incomeMessage.UsernameTarget)
                {
                    Application.Current.Dispatcher.Invoke(() => ChatMessages.Add(observableIncome));
                }
                else
                {
                    _chatModel.NewMessage(observableIncome);
                }
            }
            else if (incomeMessage.UsernameSource == _myLogin)
            {
                if (SelectedUser.Name == incomeMessage.UsernameTarget)
                {
                    Application.Current.Dispatcher.Invoke(() => ChatMessages.Add(observableIncome));
                }
            }
            else if (SelectedUser.Name == incomeMessage.UsernameSource)
            {
                Application.Current.Dispatcher.Invoke(() => ChatMessages.Add(observableIncome));
            }

            _chatModel.NewMessage(observableIncome);
        }

        private void HandleListOfMessagesReceived(object sender, ListOfMessagesReceivedEventArgs e)
        {
            foreach (Message message in e.Messages)
            {
                bool isMyMessage = message.UsernameSource == _myLogin;

                var observableMessage = new ObservableMessage
                                        {
                                            IsMyMessage = isMyMessage,
                                            Text = message.Text,
                                            UsernameSource = message.UsernameSource,
                                            Time = message.Time,
                                            UsernameTarget = message.UsernameTarget
                                        };
                _chatModel.NewMessage(observableMessage);
            }

            ChatMessages = _chatModel.GetChat(SelectedUser.Name);
        }

        private void HandleUsersStatusesRequest(object sender, UsersStatusesReceivedEventArgs e)
        {
            UsersStatusesCollection = new ObservableCollection<UserState>(e.UsersStatuses);
        }

        #endregion
    }
}
