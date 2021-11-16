using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Network.Messages
{
    class UserStatusChangeBroadcast
    {
        #region Properties

        public UserState User { get; set; }

        #endregion Properties

        #region Constructors
        public UserStatusChangeBroadcast(UserState incomeUser)
        {
            User = incomeUser;
        }
        #endregion Constructors

        #region Methods
        public MessageContainer GetContainer()
        {
            var container = new MessageContainer
            {
                Identifier = nameof(UserStatusChangeBroadcast),
                Payload = this
            };

            return container;
        }
        #endregion Methods
    }
}
