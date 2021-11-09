// ---------------------------------------------------------------------------------------------------------------------------------------------------
// Copyright ElcomPlus LLC. All rights reserved.
// Author: Пальников М. С.
// ---------------------------------------------------------------------------------------------------------------------------------------------------

namespace Common.Network
{
    using System;

    using _EventArgs_;

    public interface ITransport
    {
        #region Events

        event EventHandler<ConnectionStateChangedEventArgs> ConnectionStateChanged;
        event EventHandler<MessageReceivedEventArgs> MessageReceived;
        event EventHandler<UsersStatusesRequestEventArgs> UsersStatusesRequest;

        #endregion Events

        #region Methods

        void Connect(string address, string port);

        void Disconnect();

        void Login(string login);

        void Send(string message);

        #endregion Methods
    }
}
