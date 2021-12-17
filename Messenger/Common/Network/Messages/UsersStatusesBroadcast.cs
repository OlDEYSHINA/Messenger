namespace Common.Network.Messages
{
    using System.Collections.Generic;

    public class UsersStatusesBroadcast
    {
        #region Properties

        public List<UserState> ListOfUsersStatuses { get; set; }

        #endregion

        #region Constructors

        public UsersStatusesBroadcast(List<UserState> incomeList)
        {
            ListOfUsersStatuses = incomeList;
        }

        #endregion

        #region Methods

        public MessageContainer GetContainer()
        {
            var container = new MessageContainer
                            {
                                Identifier = nameof(UsersStatusesBroadcast),
                                Payload = this
                            };

            return container;
        }

        #endregion
    }
}
