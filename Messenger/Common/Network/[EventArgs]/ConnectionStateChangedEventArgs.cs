namespace Common.Network
{
    public class ConnectionStateChangedEventArgs
    {
        #region Properties

        public string ClientName { get; }

        public bool Connected { get; }

        public string Reason { get; }

        #endregion

        #region Constructors

        public ConnectionStateChangedEventArgs(string clientName, bool connected, string reason)
        {
            ClientName = clientName;
            Connected = connected;
            Reason = reason;
        }

        #endregion
    }
}
