using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Network.Messages
{
    public class EventLogRequest
    {
        #region Properties

        public DateTime FirstDate;
        public DateTime SecondDate;

        #endregion Properties

        #region Constructors

        public EventLogRequest(DateTime firstDate, DateTime secondDate)
        {
            FirstDate = firstDate;
            SecondDate = secondDate;
        }

        #endregion Constructors

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

        #endregion Methods
    }
}
