using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.BLL
{
    public class ObservableMessage
    {
        private string _usernameSource;
        private string _usernameTarget;
        private string _text;
        private DateTime _time;
        private bool _isMyMessage;
        public string UsernameSource
        {
            get => _usernameSource;
            set => _usernameSource = value;
        }
        public string UsernameTarget
        {
            get => _usernameTarget;
            set => _usernameTarget = value;
        }
        public string Text
        {
            get => _text;
            set => _text = value;
        }
        public DateTime Time
        {
            get => _time;
            set => _time = value;
        }
        public bool IsMyMessage
        {
            get => _isMyMessage;
            set => _isMyMessage = value;
        }
    }
}
