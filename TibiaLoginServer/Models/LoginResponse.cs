using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TibiaLoginServer.Models
{
    public class LoginResponse
    {
        public Session Session { get; set; }
        public PlayData PlayData { get; set; }
    }
}
