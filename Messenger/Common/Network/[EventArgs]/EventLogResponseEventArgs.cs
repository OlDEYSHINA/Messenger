using Common.Network.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Network._EventArgs_
{
    public class EventLogResponseEventArgs
    {
        #region Properties

        public List<EventNote> EventLog;

        #endregion Properties

        #region Constructors

        public EventLogResponseEventArgs(List<EventNote> eventLog)
        {
            EventLog = eventLog;
        }

        #endregion Constructors
    }
}
