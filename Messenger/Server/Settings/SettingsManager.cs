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

        private readonly Settings _defaultSettings = new Settings
                                                   {
                                                       ConnectionString = "data source=(localdb)\\MSSQLLocalDB;Initial " +
                                                                          "Catalog=userstore;Integrated Security=True;",
                                                       DbName = "DBConnection",
                                                       Timeout = 3000000000, //5 минут, c интервалом проверки 10 секунд
                                                       Ip = "0.0.0.0",
                                                       Port = 65000,
                                                       ProviderName = "System.Data.SqlClient"
                                                   };

        private readonly string _defaultPath = Environment.CurrentDirectory +
                                              "\\ServerSettings.json";

        #endregion

        #region Properties

        public Settings DefaultSettings => _defaultSettings;

        public int Port { get; }

        public long Timeout { get; }

        public IPAddress Ip { get; }

        public ConnectionStringSettings ConnectionSettings { get; }

        #endregion

        #region Constructors

        public SettingsManager()
        {
            Settings settings = LoadFromFile(_defaultPath);

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
                SaveToFile(_defaultSettings, _defaultPath);
                settings = LoadFromFile(_defaultPath);
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

        private void SaveToFile(Settings settings, string savePath)
        {
            Directory.CreateDirectory(Path.GetDirectoryName(savePath));
            var serializer = new JsonSerializer
                             {
                                 NullValueHandling = NullValueHandling.Include,
                                 TypeNameHandling = TypeNameHandling.All,
                                 Formatting = Formatting.Indented
                             };

            using (var sw = new StreamWriter(savePath))
            {
                using (JsonWriter writer = new JsonTextWriter(sw))
                {
                    serializer.Serialize(writer, settings);
                }
            }
        }

        private Settings LoadFromFile(string loadPath)
        {
            var settings = new Settings();

            try
            {
                var serializer = new JsonSerializer
                                 {
                                     NullValueHandling = NullValueHandling.Include,
                                     TypeNameHandling = TypeNameHandling.All,
                                     Formatting = Formatting.None
                                 };

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
                SaveToFile(_defaultSettings, _defaultPath);
                return _defaultSettings;
            }

            if (settings == null)
            {
                settings = _defaultSettings;
                SaveToFile(_defaultSettings, _defaultPath);
            }
            
            return settings;
        }

        #endregion
    }
}
