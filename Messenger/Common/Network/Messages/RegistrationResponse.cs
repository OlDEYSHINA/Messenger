namespace Common.Network.Messages
{
    internal class RegistrationResponse
    {
        #region Properties

        public RegistrationResult RegistrationResult { get; set; }

        #endregion

        #region Constructors

        public RegistrationResponse(RegistrationResult result)
        {
            RegistrationResult = result;
        }

        #endregion

        #region Methods

        public MessageContainer GetContainer()
        {
            var container = new MessageContainer
                            {
                                Identifier = nameof(RegistrationResponse),
                                Payload = this
                            };

            return container;
        }

        #endregion
    }
}
