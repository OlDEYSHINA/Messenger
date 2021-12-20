namespace Common.Network
{
    using System.Collections.Generic;

    using Messages;

    public class EventLogResponseEventArgs
    {
        #region Properties

        public List<EventNote> EventLog { get; }

        #endregion

        #region Constructors

        public EventLogResponseEventArgs(List<EventNote> eventLog)
        {
            EventLog = eventLog;
        }

        #endregion
    }
}
