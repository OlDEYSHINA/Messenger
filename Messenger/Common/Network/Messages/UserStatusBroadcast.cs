using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Network.Messages
{
    public class UserStatusBroadcast
    {
        #region Properties

        public List<UserStatus> ListOfUsersStatuses { get; set; }

        #endregion Properties
        
        #region Constructors
        public UserStatusBroadcast(List <UserStatus>incomeList)
        {
            ListOfUsersStatuses = incomeList;
        }
        #endregion Constructors

        #region Methods
        public MessageContainer GetContainer()
        {
            var container = new MessageContainer
            {
                Identifier = nameof(UserStatusBroadcast),
                Payload = this
            };

            return container;
        }
        #endregion Methods
    }
}
