using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Network._EventArgs_
{
    public class ListOfMessagesReceivedEventArgs
    {
        #region Properties

        public List<Message> Messages;

        #endregion Properties

        #region Constructors

        public ListOfMessagesReceivedEventArgs(List<Message> messages)
        {
            Messages = messages;
        }

        #endregion Constructors
    }
}
