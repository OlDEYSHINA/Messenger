namespace Common.Network.Messages
{
    public class MessageResponse
    {
        #region Properties

        public string Message { get; set; }

        #endregion

        #region Constructors

        public MessageResponse(string message)
        {
            Message = message;
        }

        #endregion

        #region Methods

        public MessageContainer GetContainer()
        {
            var container = new MessageContainer
                            {
                                Identifier = nameof(MessageResponse),
                                Payload = this
                            };

            return container;
        }

        #endregion
    }
}
