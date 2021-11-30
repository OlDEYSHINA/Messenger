using System;

namespace Common
{
    public class Message
    {
        private string usernameSource;
        private string usernameTarget;
        private string text;
        private DateTime time;

        public string UsernameSource
        {
            get => usernameSource;
            set => usernameSource = value;
        }
        public string UsernameTarget
        {
            get => usernameTarget;
            set => usernameTarget = value;
        }
        public string Text
        {
            get => text;
            set => text = value;
        }
        public DateTime Time
        {
            get => time;
            set => time = value;
        }
    }
}
