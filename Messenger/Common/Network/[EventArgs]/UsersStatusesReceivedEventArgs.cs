namespace Common.Network
{
    using System.Collections.Generic;

    public class UsersStatusesReceivedEventArgs
    {
        #region Properties

        public List<UserState> UsersStatuses { get; }

        #endregion

        #region Constructors

        public UsersStatusesReceivedEventArgs(List<UserState> incomeList)
        {
            UsersStatuses = incomeList;
        }

        #endregion
    }
}
