namespace Common.Network.Messages
{
    using System;

    public class EventNote
    {
        #region Properties

        public string Login { get; set; }

        public string EventText { get; set; }

        public DateTime Date { get; set; }

        #endregion

        #region Constructors

        public EventNote(string login, string eventText, DateTime date)
        {
            Login = login;
            EventText = eventText;
            Date = date;
        }

        #endregion
    }
}
