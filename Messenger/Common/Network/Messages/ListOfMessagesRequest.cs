namespace Common.Network.Messages
{
    public class ListOfMessagesRequest
    {
        #region Properties

        public string MyLogin { get; set; }

        public string CompanionLogin { get; set; }

        #endregion

        #region Constructors

        public ListOfMessagesRequest(string myLogin, string companionLogin)
        {
            MyLogin = myLogin;
            CompanionLogin = companionLogin;
        }

        #endregion

        #region Methods

        public MessageContainer GetContainer()
        {
            var container = new MessageContainer
                            {
                                Identifier = nameof(ListOfMessagesRequest),
                                Payload = this
                            };

            return container;
        }

        #endregion
    }
}
