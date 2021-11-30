using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Server.Database.DBModels
{
    class User
    {
        #region Properties

        [Key]
        public int Id { get; set; }

        public string Login { get; set; }
        public string Password { get; set; }

        #endregion Properties
    }
}
