namespace Server.Database.DBServices
{
    using System;
    using System.Collections.Generic;

    using Common.Network.Messages;

    internal class ClientEventService
    {
        #region Fields

        private readonly DatabaseController _databaseController;

        #endregion

        #region Constructors

        public ClientEventService(DatabaseController databaseController)
        {
            _databaseController = databaseController;
        }

        #endregion

        #region Methods

        public bool TryAddClientEvent(string login, string eventText, DateTime date)
        {
            return _databaseController.TryAddClientEvent(login, eventText, date);
        }

        public List<EventNote> GetEventLog(DateTime firstDate, DateTime secondDate)
        {
            return _databaseController.GetEventsList(firstDate, secondDate);
        }

        public List<EventNote> GetAllEventLog()
        {
            return _databaseController.GetEventLog();
        }

        #endregion
    }
}
