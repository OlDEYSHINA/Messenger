using Client.ViewModels;
using Common.Network;
using System.Windows;

namespace Client.Services
{
    class ChatMenuService
    {
        #region Fields

        ChatViewModel _chatViewModel;
        ITransport _transport;

        #endregion Fields

        #region Constructors

        public ChatMenuService()
        {
            //_chatViewModel = chatViewModel; ChatViewModel chatViewModel,ITransport transport
            //_transport = transport;
        }

        #endregion Constructors

        #region Methods

        public void Exit()
        {
            Application.Current.Shutdown();
        }
        public void Settings()
        {

        }
        public void About()
        {

        }

        #endregion Methods
    }
}
