using Client.BLL;
using Common;
using Common.Network;
using System.Collections.Concurrent;
using System.Collections.ObjectModel;
using System.Linq;

namespace Client.Models
{
    public class ChatModel
    {
        public ObservableCollection<ObservableMessage> CurrentChat = new ObservableCollection<ObservableMessage>();
        public ObservableCollection<UserState> UserStatuses = new ObservableCollection<UserState>();

        private readonly ConcurrentDictionary<string, ObservableCollection<ObservableMessage>> _chats = new ConcurrentDictionary<string, ObservableCollection<ObservableMessage>>();

        private ITransport _transport;


        private string _myLogin;
        public ChatModel(string myLogin, ITransport transport)
        {
            _transport = transport;
            _myLogin = myLogin;
            _chats.TryAdd("Global", new ObservableCollection<ObservableMessage>());
            _transport.LoadListOfMessages(myLogin, "Global");
        }
        public void AddMessageToChat(string name, ObservableMessage message)
        {
            var found = _chats.FirstOrDefault(x => x.Key == name).Value;
            found.Add(message);
        }

        public ObservableCollection<ObservableMessage> GetChat(string name)
        {
            var found = _chats.FirstOrDefault(x => x.Key == name).Value;
            if (found == null)
            {
                var dictionary = new ObservableCollection<ObservableMessage>();
                _chats.TryAdd(name, dictionary);
                _transport.LoadListOfMessages(_myLogin, name);
                //  CurrentChat = dictionary;

                found = dictionary;

            }
            ObservableCollection<ObservableMessage> observableMessages = new ObservableCollection<ObservableMessage>();
            bool isMyMessage;
            foreach (var item in found)
            {
                if (item.UsernameSource == _myLogin)
                {
                    isMyMessage = true;
                }
                else
                {
                    isMyMessage = false;
                }
                observableMessages.Add(new ObservableMessage
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
                var found = _chats.FirstOrDefault(x => x.Key == message.UsernameTarget).Value;

                App.Current.Dispatcher.Invoke(() => found.Add(message));
            }
            else if (message.UsernameSource == _myLogin)
            {
                var found = _chats.FirstOrDefault(x => x.Key == message.UsernameTarget).Value;
                App.Current.Dispatcher.Invoke(() => found.Add(message)); //ZAFICSIRUEM
            }
            else
            {
                var found = _chats.FirstOrDefault(x => x.Key == message.UsernameSource).Value;
                App.Current.Dispatcher.Invoke(() => found.Add(message));
            }

            // App.Current.Dispatcher.Invoke(() => CurrentChat.Add(message));
        }
    }
}
