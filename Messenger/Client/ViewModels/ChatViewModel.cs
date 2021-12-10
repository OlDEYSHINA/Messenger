using Client.BLL;
using Client.Models;
using Client.Services;
using Common;
using Common.Network;
using Common.Network._EventArgs_;
using Newtonsoft.Json;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.ObjectModel;
using System.Linq;

namespace Client.ViewModels
{
    class ChatViewModel : BindableBase
    {
        #region Properties
        private ITransport _transport;

        private string _message;
        private string _myLogin;
        private bool _IsDarkTheme;

        private UserState _selectedUser;

        private ChatModel _chatModel;
        private ChatMenuService _chatMenuService;
        private EventLogViewModel _eventLogViewModel;
        private SettingsViewModel _settingsViewModel;

        public bool IsDarkTheme
        {
            get
            {
                return _IsDarkTheme;
            }
            set
            {
                SetProperty(ref _IsDarkTheme, value);
            }
        }
        public UserState SelectedUser
        {
            get
            {
                return _selectedUser;
            }
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


        public ObservableCollection<ObservableMessage> ChatMessages
        {
            get
            {
                return _chatModel.CurrentChat;
            }
            set
            {
                SetProperty(ref _chatModel.CurrentChat, value);
            }
        }

        public ObservableCollection<UserState> UsersStatusesCollection
        {
            get
            {
                return _chatModel.UserStatuses;
            }
            set
            {
                SetProperty(ref _chatModel.UserStatuses, value);
            }
        }

        public string Message
        {
            get
            {
                return _message;
            }
            set
            {
                SetProperty(ref _message, value);
            }
        }
        #endregion Properties

        #region Constructors

        public ChatViewModel(MainWindowViewModel mainWindowViewModel, ITransport transport, string login)
        {
            _myLogin = login;
            _transport = transport;
            _chatModel = new ChatModel(_myLogin, _transport);
            SelectedUser = new UserState("Global", true);
            _chatMenuService = new ChatMenuService();
            _transport.MessageReceived += HandleMessageReceived;
            _transport.UsersStatusesReceived += HandleUsersStatusesRequest;
            _transport.UserStateChanged += HandleUserStateChange;
            _transport.ListOfMessagesReceived += HandleListOfMessagesReseived;

            SendMessage = new DelegateCommand(SendMessageToServer, () => true);

            MenuExitButton = new DelegateCommand(_chatMenuService.Exit, () => true);
            MenuSettingsButton = new DelegateCommand(_chatMenuService.Settings, () => true);
            MenuAboutButton = new DelegateCommand(_chatMenuService.About, () => true);
            OpenEventLog = new DelegateCommand(OpenEventLogWindow, () => true);
        }

        #endregion Constructors

        public void OpenEventLogWindow()
        {
            _eventLogViewModel = new EventLogViewModel(_transport);
            _eventLogViewModel.OpenWindow();
        }

        public void SendMessageToServer()
        {
            if (!string.IsNullOrEmpty(Message))
            {
                var message = new Common.Message { Text = Message, UsernameSource = _myLogin, UsernameTarget = SelectedUser.Name, Time = DateTime.Now };
                var serializeMessage = JsonConvert.SerializeObject(message);
                _transport?.Send(serializeMessage);
                Message = null;
            }
        }

        private void HandleUserStateChange(object sender, UserStateChangedEventArgs e)
        {
            var found = UsersStatusesCollection.FirstOrDefault(x => x.Name == e.user.Name);
            if (found != null)
            {
                var foundIndex = UsersStatusesCollection.IndexOf(found);
                App.Current.Dispatcher.Invoke(() =>
                {
                    UsersStatusesCollection.RemoveAt(foundIndex);
                    UsersStatusesCollection.Insert(foundIndex, e.user);
                    found.IsOnline = e.user.IsOnline;
                });
            }
            else
            {
                App.Current.Dispatcher.Invoke(() => UsersStatusesCollection.Add(e.user));
            }
        }
        private void HandleMessageReceived(object sender, MessageReceivedEventArgs e)
        {
            Common.Message incomeMessage = null;
            try
            {
                incomeMessage = JsonConvert.DeserializeObject<Common.Message>(e.Message);
            }
            catch
            {
                if (incomeMessage == null)
                {
                    incomeMessage = new Common.Message { Text = e.Message, UsernameTarget = "Global", Time = DateTime.Now, UsernameSource = "server" };
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
            ObservableMessage observableIncome = new ObservableMessage
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
                    App.Current.Dispatcher.Invoke(() => ChatMessages.Add(observableIncome));
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
                    App.Current.Dispatcher.Invoke(() => ChatMessages.Add(observableIncome));
                }
            }
            else if (SelectedUser.Name == incomeMessage.UsernameSource)
            {
                App.Current.Dispatcher.Invoke(() => ChatMessages.Add(observableIncome));
            }
            
                _chatModel.NewMessage(observableIncome);
            
        }
        private void HandleListOfMessagesReseived(object sender, ListOfMessagesReceivedEventArgs e)
        {
            bool isMyMessage;
            ObservableMessage observableMessage;
            foreach (var message in e.Messages)
            {
                if (message.UsernameSource == _myLogin)
                {
                    isMyMessage = true;
                }
                else
                {
                    isMyMessage = false;
                }
                observableMessage = new ObservableMessage
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

    }
}
