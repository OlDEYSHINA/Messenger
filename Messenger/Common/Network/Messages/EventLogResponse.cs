namespace Common.Network.Messages
{
    using System.Collections.Generic;

    public class EventLogResponse
    {
        #region Properties

        public List<EventNote> EventLog { get; set; }

        #endregion

        #region Constructors

        public EventLogResponse(List<EventNote> eventLog)
        {
            EventLog = eventLog;
        }

        #endregion

        #region Methods

        public MessageContainer GetContainer()
        {
            var container = new MessageContainer
                            {
                                Identifier = nameof(EventLogResponse),
                                Payload = this
                            };

            return container;
        }

        #endregion
    }
}
