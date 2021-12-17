namespace Common.Network.Messages
{
    using System;

    public class EventLogRequest
    {
        #region Properties

        public DateTime FirstDate { get; set; }

        public DateTime SecondDate { get; set; }

        #endregion

        #region Constructors

        public EventLogRequest(DateTime firstDate, DateTime secondDate)
        {
            FirstDate = firstDate;
            SecondDate = secondDate;
        }

        #endregion

        #region Methods

        public MessageContainer GetContainer()
        {
            var container = new MessageContainer
                            {
                                Identifier = nameof(EventLogRequest),
                                Payload = this
                            };

            return container;
        }

        #endregion
    }
}
