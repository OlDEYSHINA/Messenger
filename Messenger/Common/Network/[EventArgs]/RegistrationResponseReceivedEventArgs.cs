using Common.Network._Enums_;

namespace Common.Network._EventArgs_
{
    public class RegistrationResponseReceivedEventArgs
    {
        #region Properties

        public RegistrationResult RegistrationResult { get; set; }

        #endregion Properties

        #region Constructors

        public RegistrationResponseReceivedEventArgs(RegistrationResult result)
        {
            RegistrationResult = result;
        }

        #endregion Constructors
    }
}
