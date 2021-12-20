namespace Common.Network
{
    public class LoginResponseReceivedEventArgs
    {
        #region Properties

        public LoginResult LoginResult { get; }

        #endregion

        #region Constructors

        public LoginResponseReceivedEventArgs(LoginResult result)
        {
            LoginResult = result;
        }

        #endregion
    }
}
