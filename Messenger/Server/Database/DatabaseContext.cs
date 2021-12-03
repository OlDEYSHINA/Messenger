using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using Server.Database.DBModels;

namespace Server.Database
{
    class DatabaseContext : DbContext
    {
        #region Properties

        public DbSet<User> Users { get; set; }

        public DbSet<Message> Messages { get; set; }

        public DbSet<ClientEvent> ClientsEvents { get; set; }

        #endregion Properties

        #region Constructors

        public DatabaseContext(string connectionString) : base("DBConnection")
        {
            Database.Connection.ConnectionString = connectionString;
        }

        #endregion Constructors
    }
}
