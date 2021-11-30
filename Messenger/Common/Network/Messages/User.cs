using System;

namespace Common
{
    public class User
    {
        public string Name { get; set; }
        public Guid ID { get; set; }
        public User(string name, Guid id)
        {
            Name = name;
            ID = id;
        }
    }
}
