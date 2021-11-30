using System;
using System.Collections.Generic;
using System.Linq;
using Server.Database.DBModels;
using System.Text;
using System.Threading.Tasks;
using Common.Network._Enums_;

namespace Server.Database.DBServices
{
    class UserService
    {
        #region Fields

        private readonly DatabaseController _databaseController;

        #endregion Fields

        #region Constructors

        public UserService(DatabaseController databaseController)
        {
            _databaseController = databaseController;
        }

        #endregion Constructors

        #region Methods

        public bool AddUser(string username,string password)
        {
            return _databaseController.TryAddClient(username, password);
        }

        public List<string> GetUsers()
        {
            List<User> usersList = _databaseController.GetUsers();
            List<string> users = usersList.Select(x => x.Login).ToList();
            return users;
        }

        public LoginResult TryLogin(string login,string password)
        {
            return _databaseController.CheckLogin(login, password);
        }

        #endregion Methods
    }
}
