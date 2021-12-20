namespace Common.Network.Messages
{
    internal class UserStatusChangeBroadcast
    {
        #region Properties

        public UserState User { get; set; }

        #endregion

        #region Constructors

        public UserStatusChangeBroadcast(UserState incomeUser)
        {
            User = incomeUser;
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
