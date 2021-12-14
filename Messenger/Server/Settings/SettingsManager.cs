using Newtonsoft.Json;
using System;
using System.Configuration;
using System.IO;
using System.Net;

namespace Server.Settings
{
    class SettingsManager
    {
        #region Fields
        public readonly Settings DefaultSettings = new Settings
        {
            ConnectionString = "data source=(localdb)\\MSSQLLocalDB;Initial " +
               "Catalog=userstore;Integrated Security=True;",
            DBName = "DBConnection",
            Timeout = 3000000000, //5 минут, c интервалом проверки 10 секунд
            Ip = "0.0.0.0",
            Port = 65000,
            ProviderName = "System.Data.SqlClient"
        };

        private readonly string DefaultPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) +
                                                    "\\Messanger\\ServerSettings.txt";
        private int _port;
        private long _timeout;

        private IPAddress _ip;

        private ConnectionStringSettings _connectionSettings;

        #endregion Fields

        #region Properties

        public int Port
        {
            get => _port;
            set => _port = value;
        }

        public long Timeout
        {
            get => _timeout;
            set => _timeout = value;
        }

        public IPAddress Ip
        {
            get => _ip;
            set => _ip = value;
        }

        public ConnectionStringSettings ConnectionSettings
        {
            get => _connectionSettings;
            set => _connectionSettings = value;
        }

        #endregion Properties

        #region Constructors

        public SettingsManager()
        {

            var settings = LoadFromFile(DefaultPath);
            if (settings.ConnectionString != null)
            {
                Ip = IPAddress.Parse(settings.Ip);
                Port = settings.Port;
                Timeout = settings.Timeout;
                ConnectionSettings = new ConnectionStringSettings(settings.DBName,
                    settings.ConnectionString, settings.ProviderName);
            }
            else
            {
                Console.WriteLine("Файл с настройками поврежден или отсутствует,\nсоздание нового файла настроек");
                SaveToFile(DefaultSettings, DefaultPath);
                settings = LoadFromFile(DefaultPath);
                Ip = IPAddress.Parse(settings.Ip);
                Port = settings.Port;
                Timeout = settings.Timeout;
                ConnectionSettings = new ConnectionStringSettings(settings.DBName,
                    settings.ConnectionString, settings.ProviderName);
            }


        }

        #endregion Constructors

        #region Methods

        public void SaveToFile(Settings settings, string savePath)
        {
            Directory.CreateDirectory(Path.GetDirectoryName(savePath));
            JsonSerializer serializer = new JsonSerializer();
            serializer.NullValueHandling = NullValueHandling.Include;
            serializer.TypeNameHandling = TypeNameHandling.All;
            serializer.Formatting = Formatting.Indented;
            using (StreamWriter sw = new StreamWriter(savePath))
            using (JsonWriter writer = new JsonTextWriter(sw))
            {
                serializer.Serialize(writer, settings);
            };
        }

        public Settings LoadFromFile(string loadPath)
        {
            Settings settings = new Settings();
            try
            {
                JsonSerializer serializer = new JsonSerializer();
                serializer.NullValueHandling = NullValueHandling.Include;
                serializer.TypeNameHandling = TypeNameHandling.All;
                serializer.Formatting = Formatting.None;
                using (StreamReader sr = new StreamReader(loadPath))
                using (JsonReader reader = new JsonTextReader(sr))
                {
                    settings = (Settings)serializer.Deserialize<Settings>(reader);
                }
            }
            catch (Exception)
            {
                return settings;
            }
            return settings;
        }

        #endregion Methods
    }
}
