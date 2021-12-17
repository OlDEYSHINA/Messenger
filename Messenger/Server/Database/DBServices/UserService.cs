namespace Server.Database.DBServices
{
    using System.Collections.Generic;
    using System.Linq;

    using Common.Network;

    using DBModels;

    internal class UserService
    {
        #region Fields

        private readonly DatabaseController _databaseController;

        #endregion

        #region Constructors

        public UserService(DatabaseController databaseController)
        {
            _databaseController = databaseController;
        }

        #endregion

        #region Methods

        public bool AddUser(string username, string password)
        {
            return _databaseController.TryAddClient(username, password);
        }

        public List<string> GetUsers()
        {
            List<User> usersList = _databaseController.GetUsers();
            List<string> users = usersList.Select(x => x.Login).ToList();

            return users;
        }

        public LoginResult TryLogin(string login, string password)
        {
            return _databaseController.CheckLogin(login, password);
        }

        #endregion
    }
}
