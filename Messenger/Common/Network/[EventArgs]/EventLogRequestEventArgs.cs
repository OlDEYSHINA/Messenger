using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Network._EventArgs_
{
    public class EventLogRequestEventArgs
    {
        #region Properties

        public DateTime FirstDate;
        public DateTime SecondDate;
        public WsConnection Connection;

        #endregion Properties

        #region Constructors

        public EventLogRequestEventArgs(WsConnection connection, DateTime firstDate,DateTime secondTime)
        {
            Connection = connection;
            FirstDate = firstDate;
            SecondDate = secondTime;
        }

        #endregion Constructors
    }
}
