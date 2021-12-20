namespace Common.Network
{
    public class UserStateChangedEventArgs
    {
        #region Properties

        public UserState UserState { get; }

        #endregion

        #region Constructors

        public UserStateChangedEventArgs(UserState userState)
        {
            UserState = userState;
        }

        #endregion
    }
}
