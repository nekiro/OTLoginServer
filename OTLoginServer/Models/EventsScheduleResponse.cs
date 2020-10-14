using System.Collections.Generic;

namespace OTLoginServer.Models
{
    public class EventsScheduleResponse
    {
        public ICollection<Event> EventList { get; set; }
    }
}
