using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using Server;

namespace ServerService
{
    public partial class Service1 : ServiceBase
    {
        private NetworkManager _networkManager;
        public Service1()
        {
            InitializeComponent();
            
        }

        protected override void OnStart(string[] args)
        {
            _networkManager = new NetworkManager();
            _networkManager.Start();
        }

        protected override void OnStop()
        {
            _networkManager.Stop();
        }
    }
}
