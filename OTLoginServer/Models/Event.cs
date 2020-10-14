namespace OTLoginServer.Models
{
    public class Event
    {
        public long StartDate { get; set; }
        public long EndDate { get; set; }
        public string ColorLight { get; set; }
        public string ColorDark { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int DisplayPriority { get; set; } = 1;
        public bool IsSeasonal { get; set; } = false;
    }
}