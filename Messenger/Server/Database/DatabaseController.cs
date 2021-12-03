using Common.Network._Enums_;
using Common.Network.Messages;
using Server.Database.DBModels;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;

namespace Server.Database
{
    class DatabaseController
    {
        #region Fields

        private readonly DatabaseContext _databaseContext;
        private readonly string _connectionString;

        #endregion Fields

        #region Constructors

        public DatabaseController(ConnectionStringSettings connectionString)
        {
            _connectionString = connectionString.ToString();
            _databaseContext = new DatabaseContext(_connectionString);
        }

        #endregion Constructors

        #region Methods

        public bool TryAddMessage(string source, string target, string messageText, DateTime date)
        {
            try
            {
                Message message = new Message
                {
                    SourceUsername = source,
                    TargetUsername = target,
                    MessageText = messageText,
                    Date = date
                };

                using (var context = new DatabaseContext(_connectionString))
                {
                    context.Messages.Add(message);
                    context.SaveChanges();
                }
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool TryAddClientEvent(string login, string eventText, DateTime date)
        {
            try
            {
                ClientEvent message = new ClientEvent
                {
                    EventText = eventText,
                    Login = login,
                    Date = date
                };

                using (var context = new DatabaseContext(_connectionString))
                {
                    context.ClientsEvents.Add(message);
                    context.SaveChanges();
                }
                return true;
            }
            catch
            {
                return false;
            }
        }
        public bool TryAddClient(string login, string password)
        {
            var finded = _databaseContext.Users.Where(x => x.Login == login).Count();
            if (finded == 0)
            {
                User user = new User { Login = login, Password = password };
                using (var context = new DatabaseContext(_connectionString))
                {
                    context.Users.Add(user);
                    context.SaveChanges();
                }
                return true;
            }
            return false;

        }

        public List<User> GetUsers()
        {
            var users = _databaseContext.Users.ToList();
            return users;
        }

        public List<ClientEvent> GetClientEventsList()
        {
            var clientEvents = _databaseContext.ClientsEvents.ToList();
            return clientEvents;
        }

        public LoginResult CheckLogin(string login, string password)
        {
            var user = _databaseContext.Users.FirstOrDefault(x => x.Login == login);
            if (user != null)
            {
                if (user.Password == password)
                {
                    return LoginResult.Ok;
                }
                else
                {
                    return LoginResult.UnknownPassword;
                }
            }
            else
            {
                return LoginResult.UnknownUser;
            }
        }
        public List<Message> GetPrivateMessages(string myLogin, string companionLogin)
        {
            List<Message> messages = new List<Message>();
            var incomeMessages = _databaseContext.Messages.Where(x => (x.SourceUsername == myLogin &
            x.TargetUsername == companionLogin) | (x.SourceUsername == companionLogin & x.TargetUsername == myLogin));

            foreach (Message msg in incomeMessages)
            {
                messages.Add(msg);
            }
            return messages;
        }

        public List<EventNote> GetEventLog()
        {
            List<EventNote> log = new List<EventNote>();
            var allLog = _databaseContext.ClientsEvents.ToList();

            foreach (var eventik in allLog)
            {
                log.Add(new EventNote(eventik.Login, eventik.EventText, eventik.Date));
            }
            return log;
        }

        /// <summary>
        /// Использовать при загрузке пользователя.
        /// </summary>
        /// <returns>Возвращает список(List) сообщений из общего чата</returns>
        public List<Message> GetGlobalMessages()
        {
            List<Message> messages = new List<Message>();
            var incomeMessages = _databaseContext.Messages.Where(x => x.TargetUsername == "Global");
            
            foreach (Message msg in incomeMessages)
            {
                messages.Add(msg);
            }
            return messages;
        }
        #endregion Methods
    }
}
