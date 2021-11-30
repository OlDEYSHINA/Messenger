using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Settings
{
    public class Settings
    {
        #region Properties

        public string Ip { get; set; }
        public int Port { get; set; }
        public long Timeout { get; set; }
        public string DBName { get; set; }
        public string ConnectionString { get; set; }
        public string ProviderName { get; set; }

        #endregion Properties
    }
}
