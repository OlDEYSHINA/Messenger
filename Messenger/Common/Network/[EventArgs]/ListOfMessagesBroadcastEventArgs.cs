namespace Common.Network
{
    public class ListOfMessagesBroadcastEventArgs
    {
        #region Properties

        public string MyLogin { get; }

        public string CompanionLogin { get; }

        public WsConnection Connection { get; }

        #endregion

        #region Constructors

        public ListOfMessagesBroadcastEventArgs(WsConnection connection, string myLogin, string companionLogin)
        {
            Connection = connection;
            MyLogin = myLogin;
            CompanionLogin = companionLogin;
        }

        #endregion
    }
}
