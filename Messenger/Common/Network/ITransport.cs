﻿namespace Common.Network
{
    using System;

    using _EventArgs_;

    public interface ITransport
    {
        #region Events

        event EventHandler<ConnectionStateChangedEventArgs> ConnectionStateChanged;
        event EventHandler<MessageReceivedEventArgs> MessageReceived;
        event EventHandler<UsersStatusesReceivedEventArgs> UsersStatusesReceived;
        event EventHandler<UserStateChangedEventArgs> UserStateChanged;
        event EventHandler<RegistrationResponseReceivedEventArgs> RegistrationResponseReceived;
        event EventHandler<LoginResponseReceivedEventArgs> LoginResponseReceived;
        event EventHandler<ListOfMessagesReceivedEventArgs> ListOfMessagesReceived;
        event EventHandler<EventLogResponseEventArgs> EventLogResponse;



        #endregion Events

        #region Methods

        void Connect(string address, string port);

        void Disconnect();

        void Login(string login,string password);

        void Registration(string login, string password);

        void Send(string message);

        void LoadListOfMessages(string myLogin, string companionLogin);

        void EventRequest(DateTime firstDate, DateTime secondDate);

        #endregion Methods
    }
}
