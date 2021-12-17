namespace Common.Network
{
    using System.Collections.Generic;

    public class ListOfMessagesReceivedEventArgs
    {
        #region Properties

        public List<Message> Messages { get; }

        #endregion

        #region Constructors

        public ListOfMessagesReceivedEventArgs(List<Message> messages)
        {
            Messages = messages;
        }

        #endregion
    }
}
