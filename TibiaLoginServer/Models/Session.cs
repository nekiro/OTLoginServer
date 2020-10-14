namespace TibiaLoginServer.Models
{
    public class Session
    {
        public string SessionKey { get; set; }
        public long LastLoginTime { get; set; }
        public bool IsPremium { get; set; }
        public long PremiumUntil { get; set; }
        public string Status { get; set; } = "active";
        public bool ReturnerNotification { get; set; } = false;
        public bool ShowRewardNews { get; set; } = false;
        public bool IsReturner { get; set; } = false;
        public bool FpsTracking { get; set; } = false;
        public bool OptionTracking { get; set; } = false;
        public int TournamentPurchaseState { get; set; } = 0;
        public int TournamentCyclePhase { get; set; } = 0;
    }
}