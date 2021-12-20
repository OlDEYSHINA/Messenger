namespace Client.ViewModels
{
    using Prism.Mvvm;

    internal class SettingsViewModel : BindableBase
    {
        #region Fields

        private readonly ChatViewModel _chatViewModel;

        #endregion

        #region Properties

        public bool IsEnabledDarkTheme
        {
            get => _chatViewModel.IsDarkTheme;
            set
            {
                 _chatViewModel.IsDarkTheme= value;
            }
        }

        #endregion

        #region Constructors

        public SettingsViewModel(ChatViewModel chatViewModel)
        {
            _chatViewModel = chatViewModel;
        }

        #endregion
    }
}
