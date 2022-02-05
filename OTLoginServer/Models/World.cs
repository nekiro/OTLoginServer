namespace OTLoginServer.Models
{
    public class World
    {
        public int Id { get; set; } = 0;
        public string Name { get; set; } = "World";
        public string ExternalAddress { get; set; }
        public int ExternalPort { get; set; }
        public string ExternalAddressProtected { get; set; }
        public int ExternalPortProtected { get; set; }
        public string ExternalAddressUnprotected { get; set; }
        public int ExternalPortUnprotected { get; set; }
        public int PreviewState { get; set; } = 0;
        public string Location { get; set; } = "ALL"; // todo make it enum
        public int PvpType { get; set; }
        public bool IsTournamentWorld { get; set; } = false;
        public bool RestrictedStore { get; set; } = false;
        public int CurrentTournamentPhase { get; set; } = 0;
        public bool AnticheatProtection { get; set; } = false;
    }
}