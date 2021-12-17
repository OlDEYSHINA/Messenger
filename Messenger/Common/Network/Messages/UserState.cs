namespace Common
{
    public class UserState
    {
        #region Properties

        public string Name { get; set; }

        public bool IsOnline { get; set; }

        #endregion

        #region Constructors

        public UserState(string name, bool isOnline)
        {
            Name = name;
            IsOnline = isOnline;
        }

        #endregion
    }
}
