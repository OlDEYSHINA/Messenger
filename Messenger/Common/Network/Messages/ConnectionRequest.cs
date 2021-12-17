namespace Common.Network.Messages
{
    public class ConnectionRequest
    {
        #region Properties

        public string Login { get; set; }

        public string Password { get; set; }

        #endregion

        #region Constructors

        public ConnectionRequest(string login, string password)
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
                                Identifier = nameof(ConnectionRequest),
                                Payload = this
                            };

            return container;
        }

        #endregion
    }
}
