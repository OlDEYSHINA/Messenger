namespace Common.Network
{
    public class UserStateChangedEventArgs
    {
        #region Properties

        public UserState user { get; }

        #endregion

        #region Constructors

        public UserStateChangedEventArgs(UserState userStatus)
        {
            user = userStatus;
        }

        #endregion
    }
}
