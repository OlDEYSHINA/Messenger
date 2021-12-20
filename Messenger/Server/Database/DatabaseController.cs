namespace Server.Database
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Linq;

    using Common.Network;
    using Common.Network.Messages;

    using DBModels;

    internal class DatabaseController
    {
        #region Fields

        private readonly DatabaseContext _databaseContext;
        private readonly string _connectionString;

        #endregion

        #region Constructors

        public DatabaseController(ConnectionStringSettings connectionString)
        {
            _connectionString = connectionString.ToString();
            _databaseContext = new DatabaseContext(_connectionString);
        }

        #endregion

        #region Methods

        public bool TryAddMessage(string source, string target, string messageText, DateTime date)
        {
            try
            {
                var message = new Message
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
                var message = new ClientEvent
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
            catch (Exception e)
            {
                Exception s = e;

                return false;
            }
        }

        public bool TryAddClient(string login, string password)
        {
            int finded = _databaseContext.Users.Where(x => x.Login == login).Count();

            if (finded == 0)
            {
                var user = new User
                           {
                               Login = login,
                               Password = password
                           };

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
            List<User> users = _databaseContext.Users.ToList();

            return users;
        }

        public List<EventNote> GetEventsList(DateTime firstDate, DateTime secondDate)
        {
            var log = new List<EventNote>();
            List<ClientEvent> clientEvents = _databaseContext.ClientsEvents.Where(x => (x.Date >= firstDate) & (x.Date <= secondDate)).ToList();

            foreach (ClientEvent eventik in clientEvents)
            {
                log.Add(new EventNote(eventik.Login, eventik.EventText, eventik.Date));
            }

            return log;
        }

        public LoginResult CheckLogin(string login, string password)
        {
            User user = _databaseContext.Users.FirstOrDefault(x => x.Login == login);

            if (user != null)
            {
                if (user.Password == password)
                {
                    return LoginResult.Ok;
                }

                return LoginResult.UnknownPassword;
            }

            return LoginResult.UnknownUser;
        }

        public List<Message> GetPrivateMessages(string myLogin, string companionLogin)
        {
            var messages = new List<Message>();
            IQueryable<Message> incomeMessages = _databaseContext.Messages.Where(
                x => ((x.SourceUsername == myLogin) &
                      (x.TargetUsername == companionLogin)) | ((x.SourceUsername == companionLogin) & (x.TargetUsername == myLogin)));

            foreach (Message msg in incomeMessages)
            {
                messages.Add(msg);
            }

            return messages;
        }

        public List<EventNote> GetEventLog()
        {
            var log = new List<EventNote>();
            List<ClientEvent> allLog = _databaseContext.ClientsEvents.ToList();

            foreach (ClientEvent eventik in allLog)
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
            var messages = new List<Message>();
            IQueryable<Message> incomeMessages = _databaseContext.Messages.Where(x => x.TargetUsername == "Global");

            foreach (Message msg in incomeMessages)
            {
                messages.Add(msg);
            }

            return messages;
        }

        #endregion
    }
}
