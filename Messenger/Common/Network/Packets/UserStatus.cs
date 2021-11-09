namespace Common
{
    public class UserStatus
    {
        public string Name { get; set; }
        public bool IsOnline { get; set; }
        public UserStatus(string name, bool isOnline)
        {
            Name = name;
            IsOnline = isOnline;
        }
    }
}
