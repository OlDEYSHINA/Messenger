using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.Models
{
    public interface IRegistrationModel
    {
        string Username { get; set; }
        string Password { get; set; }
        string EMail { get; set; }
    }
}
