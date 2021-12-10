using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Network.Messages
{
    public class EventLogResponse
    {
        #region Properties

        public List<EventNote> EventLog;

        #endregion Properties

        #region Constructors

        public EventLogResponse(List<EventNote> eventLog)
        {
            EventLog = eventLog;
        }

        #endregion Constructors

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

        #endregion Methods
    }
}
