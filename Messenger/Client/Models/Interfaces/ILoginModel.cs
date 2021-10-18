using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.Models
{
    public interface ILoginModel
    {
        string Username { get; set; }
        string Password { get; set; }
    }
}
