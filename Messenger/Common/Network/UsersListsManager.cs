using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Network
{
    public class UsersListsManager
    {
        #region Fields

        private List<UserState> _usersStatuses = new List<UserState>();
        private List<User> _usersGuid = new List<User>();
        private WsServer _wsServer;
        #endregion Fields

        #region Constructors

        public UsersListsManager(WsServer server)
        {
            _wsServer = server;
        }

        #endregion Constructors
        #region Methods
        public void AddToLists(string login, Guid id)
        {
            User addUser = new User(login, id);
            _usersGuid.Add(addUser);
            if (_usersStatuses.Find(x => x.Name == login) == null)
            {
                UserState userStatus = new UserState(login, true);
                _usersStatuses.Add(userStatus);
            }
            else
            {
                _usersStatuses.Find(x => x.Name == login).IsOnline = true;
            }
        }
       

        /// <summary>
        /// Удаление пользователя из листа сервера и смена состояния в листе клиента
        /// </summary>
        /// <param name="id">Guid пользователя</param>
        public void DeleteFromLists(Guid id)
        {
            var findedUser = _usersGuid.Find(x => x.ID == id);
            _usersGuid.Remove(findedUser);
            _usersStatuses.Find(x => x.Name == findedUser.Name).IsOnline = false;
        }

        public List<UserState> GetUsersStatuses()
        {
            return _usersStatuses;
        }

        public List<User> GetUsersGuid()
        {
            return _usersGuid;
        }
        public string GetUserName(Guid id)
        {
            var findedUser = _usersGuid.Find(x => x.ID == id);
            return findedUser.Name;
        }
        #endregion Methods
    }
}
