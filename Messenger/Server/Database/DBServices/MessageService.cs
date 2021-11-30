using Server.Database.DBModels;
using System;
using System.Collections.Generic;

namespace Server.Database.DBServices
{
    class MessageService
    {
        #region Fields

        private readonly DatabaseController _databaseController;

        #endregion Fields

        #region Constructors

        public MessageService(DatabaseController databaseController)
        {
            _databaseController = databaseController;
        }

        #endregion Constructors

        #region Methods

        public bool TryAddMessage(string sourceLogin, string targetLogin, string message, DateTime date)
        {
            return _databaseController.TryAddMessage(sourceLogin, targetLogin, message, date);
        }

        public List<Message> GetGlobalMessages()
        {
            return _databaseController.GetGlobalMessages();
        }

        public List<Message> GetPrivateMessages(string myLogin,string companionLogin)
        {
            return _databaseController.GetPrivateMessages(myLogin, companionLogin);
        }
        #endregion Methods
    }
}
