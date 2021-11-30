using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Network.Messages
{
    public class ListOfMessages
    {
        #region Properties

        public List<Message> Messages { get; set; }

        #endregion Properties

        #region Constructors

        public ListOfMessages(List<Message> messages)
        {
            Messages = messages;
        }

        #endregion Constructors

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

        #endregion Methods
    }
}

