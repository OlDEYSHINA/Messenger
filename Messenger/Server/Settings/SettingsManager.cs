namespace Server.Settings
{
    using System;
    using System.Configuration;
    using System.IO;
    using System.Net;

    using Newtonsoft.Json;

    internal class SettingsManager
    {
        #region Fields

        public readonly Settings DefaultSettings = new Settings
                                                   {
                                                       ConnectionString = "data source=(localdb)\\MSSQLLocalDB;Initial " +
                                                                          "Catalog=userstore;Integrated Security=True;",
                                                       DbName = "DBConnection",
                                                       Timeout = 3000000000, //5 минут, c интервалом проверки 10 секунд
                                                       Ip = "0.0.0.0",
                                                       Port = 65000,
                                                       ProviderName = "System.Data.SqlClient"
                                                   };

        private readonly string DefaultPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) +
                                              "\\Messanger\\ServerSettings.txt";

        #endregion

        #region Properties

        public int Port { get; set; }

        public long Timeout { get; set; }

        public IPAddress Ip { get; set; }

        public ConnectionStringSettings ConnectionSettings { get; set; }

        #endregion

        #region Constructors

        public SettingsManager()
        {
            Settings settings = LoadFromFile(DefaultPath);

            if (settings.ConnectionString != null)
            {
                Ip = IPAddress.Parse(settings.Ip);
                Port = settings.Port;
                Timeout = settings.Timeout;
                ConnectionSettings = new ConnectionStringSettings(
                    settings.DbName,
                    settings.ConnectionString,
                    settings.ProviderName);
            }
            else
            {
                Console.WriteLine("Файл с настройками поврежден или отсутствует,\nсоздание нового файла настроек");
                SaveToFile(DefaultSettings, DefaultPath);
                settings = LoadFromFile(DefaultPath);
                Ip = IPAddress.Parse(settings.Ip);
                Port = settings.Port;
                Timeout = settings.Timeout;
                ConnectionSettings = new ConnectionStringSettings(
                    settings.DbName,
                    settings.ConnectionString,
                    settings.ProviderName);
            }
        }

        #endregion

        #region Methods

        public void SaveToFile(Settings settings, string savePath)
        {
            Directory.CreateDirectory(Path.GetDirectoryName(savePath));
            var serializer = new JsonSerializer();
            serializer.NullValueHandling = NullValueHandling.Include;
            serializer.TypeNameHandling = TypeNameHandling.All;
            serializer.Formatting = Formatting.Indented;

            using (var sw = new StreamWriter(savePath))
            {
                using (JsonWriter writer = new JsonTextWriter(sw))
                {
                    serializer.Serialize(writer, settings);
                }
            }

            ;
        }

        public Settings LoadFromFile(string loadPath)
        {
            var settings = new Settings();

            try
            {
                var serializer = new JsonSerializer();
                serializer.NullValueHandling = NullValueHandling.Include;
                serializer.TypeNameHandling = TypeNameHandling.All;
                serializer.Formatting = Formatting.None;

                using (var sr = new StreamReader(loadPath))
                {
                    using (JsonReader reader = new JsonTextReader(sr))
                    {
                        settings = serializer.Deserialize<Settings>(reader);
                    }
                }
            }
            catch (Exception)
            {
                return settings;
            }

            return settings;
        }

        #endregion
    }
}
