using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Network.Messages
{
    public class EventNote
    {
        public string Login { get; set; }
        public string EventText { get; set; }
        public DateTime Date { get; set; }
        public EventNote(string login,string eventText, DateTime date)
        {
            Login = login;
            EventText = eventText;
            Date = date;
        }
    }
}
