namespace Client.BLL
{
    using System;

    public class ObservableMessage
    {
        #region Properties

        public string UsernameSource { get; set; }

        public string UsernameTarget { get; set; }

        public string Text { get; set; }

        public DateTime Time { get; set; }

        public bool IsMyMessage { get; set; }

        #endregion
    }
}
