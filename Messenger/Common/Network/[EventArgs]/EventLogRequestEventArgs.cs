namespace Common.Network
{
    using System;

    public class EventLogRequestEventArgs
    {
        #region Properties

        public DateTime FirstDate { get; }

        public DateTime SecondDate { get; }

        public WsConnection Connection { get; }

        #endregion

        #region Constructors

        public EventLogRequestEventArgs(WsConnection connection, DateTime firstDate, DateTime secondTime)
        {
            Connection = connection;
            FirstDate = firstDate;
            SecondDate = secondTime;
        }

        #endregion
    }
}
