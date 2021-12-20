namespace Server.Settings
{
    public class Settings
    {
        #region Properties

        public string Ip { get; set; }

        public int Port { get; set; }

        public long Timeout { get; set; }

        public string DbName { get; set; }

        public string ConnectionString { get; set; }

        public string ProviderName { get; set; }

        #endregion
    }
}
