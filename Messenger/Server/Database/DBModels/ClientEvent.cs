namespace Server.Database.DBModels
{
    using System;
    using System.ComponentModel.DataAnnotations;

    internal class ClientEvent
    {
        #region Properties

        [Key]
        public int Id { get; set; }

        public string Login { get; set; }

        public string EventText { get; set; }

        public DateTime Date { get; set; }

        #endregion
    }
}
