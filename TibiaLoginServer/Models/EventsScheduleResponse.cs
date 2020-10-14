using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TibiaLoginServer.Models
{
    public class EventsScheduleResponse
    {
        public ICollection<Event> EventList { get; set; }
    }
}
