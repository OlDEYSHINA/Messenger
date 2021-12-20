namespace Common.Network
{
    using System;

    using Messages;

    public class CheckLoginEventArgs
    {
        #region Properties

        public string Login { get; }

        public string Password { get; }

        public WsConnection Connection { get; }

        public Guid ClientId { get; }

        public ConnectionResponse ConnectionResponse { get; }

        #endregion

        #region Constructors

        public CheckLoginEventArgs(
            string login,
            string password,
            WsConnection connection,
            Guid clientId,
            ConnectionResponse connectionResponse)
        {
            Login = login;
            Password = password;
            Connection = connection;
            ClientId = clientId;
            ConnectionResponse = connectionResponse;
        }

        #endregion
    }
}
