namespace Common.Network.Messages
{
    internal class RegistrationRequest
    {
        #region Properties

        public string Login { get; set; }

        public string Password { get; set; }

        #endregion

        #region Constructors

        public RegistrationRequest(string login, string password)
        {
            Login = login;
            Password = password;
        }

        #endregion

        #region Methods

        public MessageContainer GetContainer()
        {
            var container = new MessageContainer
                            {
                                Identifier = nameof(RegistrationRequest),
                                Payload = this
                            };

            return container;
        }

        #endregion
    }
}
