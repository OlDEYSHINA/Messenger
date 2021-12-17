namespace Server.Database.DBModels
{
    using System.ComponentModel.DataAnnotations;

    internal class User
    {
        #region Properties

        [Key]
        public int Id { get; set; }

        public string Login { get; set; }

        public string Password { get; set; }

        #endregion
    }
}
