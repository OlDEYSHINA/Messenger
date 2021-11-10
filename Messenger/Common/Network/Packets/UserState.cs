namespace Common
{
    public class UserState
    {
        public string Name { get; set; }
        public bool IsOnline { get; set; }
        public UserState(string name, bool isOnline)
        {
            Name = name;
            IsOnline = isOnline;
        }
    }
}
