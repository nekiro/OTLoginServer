using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TibiaLoginServer.Models
{
    public class Account
    {
        public int Id { get; set; }
        public long PremiumUntil { get; set; }
        public long LastLoginTime { get; set; }
        public bool IsPremium { get; set; }
    }
}
