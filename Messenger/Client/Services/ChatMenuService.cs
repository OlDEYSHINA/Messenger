namespace Client.Services
{
    using System.Windows;

    using Common.Network;

    using View;

    using ViewModels;

    internal class ChatMenuService
    {
        #region Fields

        private ChatViewModel _chatViewModel;
        private ITransport _transport;

        #endregion

        #region Constructors

        #endregion

        #region Methods

        public void Exit()
        {
            Application.Current.Shutdown();
        }

        public void Settings()
        {
            var settingsWindow = new SettingsWindow();
            settingsWindow.Show();
        }

        public void About()
        {
        }

        #endregion
    }
}
