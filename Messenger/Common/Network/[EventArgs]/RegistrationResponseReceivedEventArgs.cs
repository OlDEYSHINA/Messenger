namespace Common.Network
{
    public class RegistrationResponseReceivedEventArgs
    {
        #region Properties

        public RegistrationResult RegistrationResult { get; }

        #endregion

        #region Constructors

        public RegistrationResponseReceivedEventArgs(RegistrationResult result)
        {
            RegistrationResult = result;
        }

        #endregion
    }
}
