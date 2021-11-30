﻿using Common.Network._Enums_;
namespace Common.Network._EventArgs_
{
    public class ConnectionStateChangedEventArgs
    {
        #region Properties

        public string ClientName { get; }
        public bool Connected { get; }
        public string Reason { get; }

        #endregion Properties

        #region Constructors

        //public ConnectionStateChangedEventArgs(string clientName, bool connected)
        //{
        //    ClientName = clientName;
        //    Connected = connected;
        //}
        public ConnectionStateChangedEventArgs(string clientName, bool connected,string reason)
        {
            ClientName = clientName;
            Connected = connected;
            Reason = reason;
        }

        #endregion Constructors
    }
}