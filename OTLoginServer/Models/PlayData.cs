using System.Collections.Generic;

namespace OTLoginServer.Models
{
    public class PlayData
    {
        public ICollection<World> Worlds { get; set; }
        public ICollection<Character> Characters { get; set; }
    }
}