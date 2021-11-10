using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Network.Messages
{
    public class UsersStatusesBroadcast
    {
        #region Properties

        public List<UserState> ListOfUsersStatuses { get; set; }

        #endregion Properties
        
        #region Constructors
        public UsersStatusesBroadcast(List <UserState>incomeList)
        {
            ListOfUsersStatuses = incomeList;
        }
        #endregion Constructors

        #region Methods
        public MessageContainer GetContainer()
        {
            var container = new MessageContainer
            {
                Identifier = nameof(UsersStatusesBroadcast),
                Payload = this
            };

            return container;
        }
        #endregion Methods
    }
}
