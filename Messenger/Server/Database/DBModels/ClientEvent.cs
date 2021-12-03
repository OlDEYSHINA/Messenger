using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Database.DBModels
{
    public class ClientEvent
    {
        #region Properties

        [Key]
        public int Id { get; set; }
        public string Login { get; set; }
        public string EventText { get; set; }
        public DateTime Date { get; set; }

        #endregion Properties
    }
}
