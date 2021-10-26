using Client.BLL.Interfaces;
using Client.BLL;
using Prism.Commands;
using Prism.Mvvm;
using System.Windows;

namespace Client.ViewModels
{
    class ChatViewModel : BindableBase
    {
        INetworkManager _networkManager;
        private string _message;
        private string _incomeMessage;
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
        public ChatViewModel(MainWindowViewModel mainWindowViewModel, INetworkManager networkManager)
        {
            _networkManager = networkManager;
            _networkManager.MessageRecieved += SaveToList;
            SendMessage = new DelegateCommand(SendMessageToServer, () => true);
        }
       
        public void SendMessageToServer()
        {
            if (!string.IsNullOrEmpty(_message))
            {
                _networkManager.SendMessage(_message);
                Message = null;
            }
        }
        public void SaveToList(object sender, ChatMessageEventArgs e)
        {
            MessageBox.Show(e.Text);
        }
    }
}
