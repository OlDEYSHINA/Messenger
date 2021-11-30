using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Network.Messages
{
    class RegistrationRequest
    {
        #region Properties

        public string Login { get; set; }
        public string Password { get; set; }

        #endregion Properties

        #region Constructors

        public RegistrationRequest(string login,string password)
        {
            Login = login;
            Password = password;
        }

        #endregion Constructors

        #region Methods

        public MessageContainer GetContainer()
        {
            var container = new MessageContainer
            {
                Identifier = nameof(RegistrationRequest),
                Payload = this
            };

            return container;
        }

        #endregion Methods
    }
}
