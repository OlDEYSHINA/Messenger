namespace Common
{
    using System;

    public class Message
    {
        #region Properties

        public string UsernameSource { get; set; }

        public string UsernameTarget { get; set; }

        public string Text { get; set; }

        public DateTime Time { get; set; }

        #endregion
    }
}
