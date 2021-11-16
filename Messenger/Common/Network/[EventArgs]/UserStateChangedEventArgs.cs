namespace Common.Network
{
    public class UserStateChangedEventArgs
    {

        #region Properties

        public UserState user { get; set; }

        #endregion Properties

        #region Constructors

        public UserStateChangedEventArgs(UserState userStatus)
        {
            user = userStatus;
        }

        #endregion Constructors
    }
}
