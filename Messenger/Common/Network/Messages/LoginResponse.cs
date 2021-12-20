namespace Common.Network.Messages
{
    internal class LoginResponse
    {
        #region Properties

        public LoginResult LoginResult { get; set; }

        #endregion

        #region Constructors

        public LoginResponse(LoginResult result)
        {
            LoginResult = result;
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
