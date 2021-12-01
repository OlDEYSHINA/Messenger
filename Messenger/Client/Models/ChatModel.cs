using Common;
using System.Collections.Concurrent;
using System.Collections.ObjectModel;
using System.Linq;
using Common.Network;


namespace Client.Models
{
    public class ChatModel
    {
        public ObservableCollection<Message> CurrentChat = new ObservableCollection<Message>();
        public ObservableCollection<UserState> UserStatuses = new ObservableCollection<UserState>();
        
        private readonly ConcurrentDictionary<string, ObservableCollection<Message>> _chats = new ConcurrentDictionary<string, ObservableCollection<Message>>();

        private ITransport _transport;


        private string _myLogin;
        public ChatModel(string myLogin, ITransport transport)
        {
            _transport = transport;
            _myLogin = myLogin;
            _chats.TryAdd("Global", new ObservableCollection<Message>());
            _transport.LoadListOfMessages(myLogin, "Global");
        }
        public void AddMessageToChat(string name,Message message)
        {
            var found = _chats.FirstOrDefault(x => x.Key == name).Value;
            found.Add(message);
        }

        public ObservableCollection<Message> GetChat(string name)
        {
            var found = _chats.FirstOrDefault(x => x.Key == name).Value;
            if (found == null)
            {
                var dictionary = new ObservableCollection<Message>();
                _chats.TryAdd(name, dictionary);
                _transport.LoadListOfMessages(_myLogin, name);
              //  CurrentChat = dictionary;
                found = dictionary;
                
            }
            return found;
        }
        
        public void NewMessage(Message message)
        {
            if (message.UsernameTarget == "Global")
            {
                var found = _chats.FirstOrDefault(x => x.Key == message.UsernameTarget).Value;

                App.Current.Dispatcher.Invoke(() => found.Add(message));
            }
            else if (message.UsernameSource == _myLogin)
            {
                var found = _chats.FirstOrDefault(x => x.Key == message.UsernameTarget).Value;
                App.Current.Dispatcher.Invoke(() => found.Add(message));
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
