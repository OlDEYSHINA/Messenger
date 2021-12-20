namespace Common.Network
{
    using System.Collections.Generic;

    public class UsersStatusesReceivedEventArgs
    {
        #region Properties

        public List<UserState> UsersState { get; }

        #endregion

        #region Constructors

        public UsersStatusesReceivedEventArgs(List<UserState> usersState)
        {
            UsersState = usersState;
        }

        #endregion
    }
}
