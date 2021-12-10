using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.ViewModels
{
    class SettingsViewModel:BindableBase
    {
        private bool _isEnabledEventLog;
        private bool _isEnabledDarkTheme;

        public bool IsEnabledEventLog
        {
            get
            {
                return _isEnabledEventLog;
            }
            set
            {
                SetProperty(ref _isEnabledEventLog, value);
            }
        }
        public bool IsEnabledDarkTheme
        {
            get
            {
                return _isEnabledDarkTheme;
            }
            set
            {
                SetProperty(ref _isEnabledDarkTheme, value);
            }
        }
    }
}
