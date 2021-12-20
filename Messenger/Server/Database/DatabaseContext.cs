namespace Server.Database
{
    using System.Data.Entity;

    using DBModels;

    internal class DatabaseContext : DbContext
    {
        #region Properties

        public DbSet<User> Users { get; set; }

        public DbSet<Message> Messages { get; set; }

        public DbSet<ClientEvent> ClientsEvents { get; set; }

        #endregion

        #region Constructors

        public DatabaseContext(string connectionString)
            : base("DBConnection")
        {
            Database.Connection.ConnectionString = connectionString;
        }

        #endregion
    }
}
