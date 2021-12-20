namespace Server.Database.DBServices
{
    using System;
    using System.Collections.Generic;

    using DBModels;

    internal class MessageService
    {
        #region Fields

        private readonly DatabaseController _databaseController;

        #endregion

        #region Constructors

        public MessageService(DatabaseController databaseController)
        {
            _databaseController = databaseController;
        }

        #endregion

        #region Methods

        public bool TryAddMessage(string sourceLogin, string targetLogin, string message, DateTime date)
        {
            return _databaseController.TryAddMessage(sourceLogin, targetLogin, message, date);
        }

        public List<Message> GetGlobalMessages()
        {
            return _databaseController.GetGlobalMessages();
        }

        public List<Message> GetPrivateMessages(string myLogin, string companionLogin)
        {
            return _databaseController.GetPrivateMessages(myLogin, companionLogin);
        }

        #endregion
    }
}
