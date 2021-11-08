using Client.BLL;
using Client.BLL.Interfaces;
using Client.Models;
using Common;
using Common.Network;
using Prism.Commands;
using Prism.Mvvm;
using System.Collections.ObjectModel;
using System.Windows;
using Newtonsoft.Json;

namespace Client.ViewModels
{
    class ChatViewModel : BindableBase
    {
        private ITransport _transport;
        private string _message;
        private string _incomeMessage;
        private ChatModel _chatModel;
        public ObservableCollection<Message> ChatMessages
        {
            get
            {
                return _chatModel.ChatMessages;
            }
            set
            {
                SetProperty(ref _chatModel.ChatMessages, value);
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
        public string IncomeMessage
        {
            get
            {
                return _incomeMessage;
            }
            set
            {
                SetProperty(ref _incomeMessage, value);
            }
        }
        public DelegateCommand SendMessage { get; }
        public ChatViewModel(MainWindowViewModel mainWindowViewModel, ITransport transport)
        {
            _transport = transport;
            _transport.MessageReceived += HandleMessageReceived;
            _chatModel = new ChatModel();
      //      _chatModel.NewMessage(new Common.Message { Text = "test", Time = System.DateTime.Now, UsernameTarget = "all", UsernameSource = "Biba" });
         
            SendMessage = new DelegateCommand(SendMessageToServer, () => true);
        }

        private void HandleMessageReceived(object sender, MessageReceivedEventArgs e)
        {
            
            var incomeMessage = JsonConvert.DeserializeObject<Message>(e.Message);
            _chatModel.NewMessage(new Common.Message { Text = incomeMessage.Text, Time = incomeMessage.Time, UsernameTarget = incomeMessage.UsernameTarget, UsernameSource = incomeMessage.UsernameSource });
        }

        public void SendMessageToServer()
        {
            if (!string.IsNullOrEmpty(Message))
            {
                _transport?.Send(Message);
              //  ChatMessages.Add(new Common.Message { Text = _message, Time = System.DateTime.Now, UsernameTarget = "all", UsernameSource = "aboba" });
                Message = null;
            }
        }
        public void SaveToList(object sender, ChatMessageEventArgs e)
        {
            MessageBox.Show(e.Text);
        }
    }
}
