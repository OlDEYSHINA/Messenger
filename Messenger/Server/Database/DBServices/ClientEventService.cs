using Common.Network.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Database.DBServices
{
    class ClientEventService
    {
        #region Fields

        private readonly DatabaseController _databaseController;

        #endregion Fields

        #region Constructors

        public ClientEventService(DatabaseController databaseController)
        {
            _databaseController = databaseController;
        }

        #endregion Constructors

        #region Methods

        public bool TryAddClientEvent(string login, string eventText, DateTime date)
        {
            return _databaseController.TryAddClientEvent(login, eventText, date);
        }

        public List<EventNote> GetAllEventLog()
        {
            return _databaseController.GetEventLog();
        }
        #endregion Methods
    }
}
