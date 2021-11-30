using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Network._Enums_;

namespace Common.Network.Messages
{
    class RegistrationResponse
    {
        #region Properties

        public RegistrationResult RegistrationResult { get; set; }

        #endregion Properties

        #region Constructors

        public RegistrationResponse(RegistrationResult result)
        {
            RegistrationResult = result;
        }

        #endregion Constructors

        #region Methods

        public MessageContainer GetContainer()
        {
            var container = new MessageContainer
            {
                Identifier = nameof(RegistrationResponse),
                Payload = this
            };

            return container;
        }

        #endregion Methods
    }
}
