namespace Common.Network
{
    using System;
    using System.Collections.Generic;

    public class UsersListsManager
    {
        #region Fields

        private readonly List<UserState> _usersStatuses;
        private readonly List<User> _usersGuid;

        #endregion

        #region Constructors

        public UsersListsManager()
        {
            _usersStatuses = new List<UserState>();
            _usersGuid = new List<User>();
            _usersStatuses.Add(new UserState("Global", true));
        }

        #endregion

        #region Methods

        public void AddToLists(string login, Guid id)
        {
            var addUser = new User(login, id);
            _usersGuid.Add(addUser);

            if (_usersStatuses.Find(x => x.Name == login) == null)
            {
                var userStatus = new UserState(login, true);
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
        /// <param name = "id">Guid пользователя</param>
        public void DeleteFromLists(Guid id)
        {
            User findedUser = _usersGuid.Find(x => x.Id == id);

            if (findedUser == null)
            {
                return;
            }

            _usersGuid.Remove(findedUser);
            _usersStatuses.Find(x => x.Name == findedUser.Name).IsOnline = false;
        }

        public void LoadListFromDb(List<string> users)
        {
            foreach (string user in users)
            {
                _usersStatuses.Add(new UserState(user, false));
            }
        }

        public bool IsUserOnline(string name)
        {
            return _usersStatuses.Find(x => x.Name == name).IsOnline;
        }

        public List<UserState> GetUsersStatuses()
        {
            return _usersStatuses;
        }

        public Guid GetUserGuid(string name)
        {
            return _usersGuid.Find(x => x.Name == name).Id;
        }

        public string GetUserName(Guid id)
        {
            return _usersGuid.Find(x => x.Id == id)?.Name;
        }

        #endregion
    }
}
