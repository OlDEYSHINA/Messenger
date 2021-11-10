using Common;
using System.Collections.ObjectModel;

namespace Client.Models
{
    class ChatModel
    {
        public ObservableCollection<Message> ChatMessages = new ObservableCollection<Message>();
        public ObservableCollection<UserState> UserStatuses = new ObservableCollection<UserState>();

        public void NewMessage(Message message)
        {
            App.Current.Dispatcher.Invoke(() => ChatMessages.Add(message));
        }
        

    }
}
