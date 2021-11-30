namespace Common.Network.Messages
{
    public class MessageBroadcast
    {
        #region Properties

        public string Message { get; set; }

        #endregion Properties

        #region Constructors

        public MessageBroadcast(string message)
        {
            Message = message;
        }

        #endregion Constructors

        #region Methods

        public MessageContainer GetContainer()
        {
            var container = new MessageContainer
            {
                Identifier = nameof(MessageBroadcast),
                Payload = this
            };

            return container;
        }

        #endregion Methods
    }
}
