using System;
using System.ComponentModel.DataAnnotations;

namespace Server.Database.DBModels
{
    public class Message
    {
        #region Properties

        [Key]
        public int Id { get; set; }

        public string SourceUsername { get; set; }
        public string TargetUsername { get; set; }
        public string MessageText { get; set; }

        public DateTime Date { get; set; }
        #endregion Properties
    }
}
