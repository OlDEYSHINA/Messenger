namespace Client.Models
{
    using System.Collections.Concurrent;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Windows;

    using BLL;

    using Common;
    using Common.Network;

    public class ChatModel
    {
        #region Fields

        public ObservableCollection<ObservableMessage> CurrentChat;
        public ObservableCollection<UserState> UserStatuses;

        private readonly ConcurrentDictionary<string, ObservableCollection<ObservableMessage>> _chats;

        private readonly ITransport _transport;

        private readonly string _myLogin;

        #endregion

        #region Constructors

        public ChatModel(string myLogin, ITransport transport)
        {
            _chats = new ConcurrentDictionary<string, ObservableCollection<ObservableMessage>>();
            UserStatuses = new ObservableCollection<UserState>();
            CurrentChat = new ObservableCollection<ObservableMessage>();
            _transport = transport;
            _myLogin = myLogin;
            _chats.TryAdd("Global", new ObservableCollection<ObservableMessage>());
            _transport.LoadListOfMessages(myLogin, "Global");
        }

        #endregion

        #region Methods

        public ObservableCollection<ObservableMessage> GetChat(string name)
        {
            ObservableCollection<ObservableMessage> found = _chats.FirstOrDefault(x => x.Key == name).Value;

            if (found == null)
            {
                found = new ObservableCollection<ObservableMessage>();
                _chats.TryAdd(name, found);
                _transport.LoadListOfMessages(_myLogin, name);
            }

            var observableMessages = new ObservableCollection<ObservableMessage>();
            bool isMyMessage;

            foreach (ObservableMessage item in found)
            {
                if (item.UsernameSource == _myLogin)
                {
                    isMyMessage = true;
                }
                else
                {
                    isMyMessage = false;
                }

                observableMessages.Add(
                    new ObservableMessage
                    {
                        IsMyMessage = isMyMessage,
                        Text = item.Text,
                        Time = item.Time,
                        UsernameSource = item.UsernameSource,
                        UsernameTarget = item.UsernameTarget
                    });
            }

            return observableMessages;
        }

        public void NewMessage(ObservableMessage message)
        {
            if (message.UsernameTarget == "Global")
            {
                ObservableCollection<ObservableMessage> found = _chats.FirstOrDefault(x => x.Key == message.UsernameTarget).Value;

                Application.Current.Dispatcher.Invoke(() => found.Add(message));
            }
            else if (message.UsernameSource == _myLogin)
            {
                ObservableCollection<ObservableMessage> found;
                found = _chats.FirstOrDefault(x => x.Key == message.UsernameTarget).Value;

                if (found == null)
                {
                    GetChat(message.UsernameTarget);
                    found = _chats.FirstOrDefault(x => x.Key == message.UsernameTarget).Value;
                }

                Application.Current.Dispatcher.Invoke(() => found.Add(message)); //ZAFICSIRUEM
            }
            else
            {
                ObservableCollection<ObservableMessage> found = _chats.FirstOrDefault(x => x.Key == message.UsernameSource).Value;
                Application.Current.Dispatcher.Invoke(() => found.Add(message));
            }

            // App.Current.Dispatcher.Invoke(() => CurrentChat.Add(message));
        }

        #endregion
    }
}
