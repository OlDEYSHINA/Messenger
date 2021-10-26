using System;
namespace Client.BLL
{
    public class ChatMessageEventArgs : Common.Message
    {
        public string Address { get; set; }

        public ChatMessageEventArgs(string address, string usernameTarget, string usernameSource, string text, DateTime time)
        {
            Address = address;
            UsernameSource = usernameSource;
            UsernameTarget = usernameTarget;
            Text = text;
            Time = time;
        }

    }
}
