using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TibiaLoginServer.Models
{
    public class CacheInfoResponse
    {
        public int PlayersOnline { get; set; }
        public int TwitchStreams { get; set; }
        public int TwitchViewer { get; set; }
        public int GamingYoutubeStreams { get; set; }
        public int GamingYoutubeViewer { get; set; }
    }
}
