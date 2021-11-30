using Common.Network._Enums_;

namespace Common.Network.Messages
{
    class LoginResponse
    {
        #region Properties

        public LoginResult LoginResult { get; set; }

        #endregion Properties

        #region Constructors

        public LoginResponse(LoginResult result)
        {
            LoginResult = result;
        }

        #endregion Constructors

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

        #endregion Methods
    }
}
