namespace Common.Network.Messages
{
    using System.Collections.Generic;

    public class ListOfMessages
    {
        #region Properties

        public List<Message> Messages { get; set; }

        #endregion

        #region Constructors

        public ListOfMessages(List<Message> messages)
        {
            Messages = messages;
        }

        #endregion

        #region Methods

        public MessageContainer GetContainer()
        {
            var container = new MessageContainer
                            {
                                Identifier = nameof(ListOfMessages),
                                Payload = this
                            };

            return container;
        }

        #endregion
    }
}
