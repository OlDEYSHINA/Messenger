namespace Common
{
    using System;

    public class User
    {
        #region Properties

        public string Name { get; set; }

        public Guid Id { get; set; }

        #endregion

        #region Constructors

        public User(string name, Guid id)
        {
            Name = name;
            Id = id;
        }

        #endregion
    }
}
