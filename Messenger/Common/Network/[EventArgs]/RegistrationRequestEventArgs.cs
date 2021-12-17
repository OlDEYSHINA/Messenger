namespace Common.Network
{
    public class RegistrationRequestEventArgs
    {
        #region Properties

        public string Login { get; }

        public string Password { get; }

        public WsConnection Connection { get; }

        #endregion

        #region Constructors

        public RegistrationRequestEventArgs(string login, string password, WsConnection connection)
        {
            Login = login;
            Password = password;
            Connection = connection;
        }

        #endregion
    }
}
