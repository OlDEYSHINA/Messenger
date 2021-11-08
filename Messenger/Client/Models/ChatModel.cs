using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;
using System.Collections.ObjectModel;
using System.IO;

namespace Client.Models
{
    class ChatModel
    {
        public ObservableCollection<Message> ChatMessages = new ObservableCollection<Message> ();
        public void NewMessage(Message message)
        {
            App.Current.Dispatcher.Invoke(() => ChatMessages.Add(message));
            
        }
        
    }
}
