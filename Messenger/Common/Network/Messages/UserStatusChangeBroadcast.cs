namespace Common.Network.Messages
{
    internal class UserStatusChangeBroadcast
    {
        #region Properties

        public UserState UserState { get; set; }

        #endregion

        #region Constructors

        public UserStatusChangeBroadcast(UserState userState)
        {
            UserState = userState;
        }

        #endregion

        #region Methods

        public MessageContainer GetContainer()
        {
            var container = new MessageContainer
                            {
                                Identifier = nameof(UserStatusChangeBroadcast),
                                Payload = this
                            };

            return container;
        }

        #endregion
    }
}
