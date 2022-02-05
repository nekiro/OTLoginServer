namespace OTLoginServer.Models
{
    public class Session
    {        
        public bool FpsTracking { get; set; } = false;
        public bool OptionTracking { get; set; } = false;
        public bool IsReturner { get; set; } = false;
        public bool ReturnerNotification { get; set; } = false;
        public bool ShowRewardNews { get; set; } = false;
        public int TournamentTicketPurchaseState { get; set; } = 0;
        public bool EmailCodeRequest { get; set; } = false;
        public string SessionKey { get; set; }
        public int LastLoginTime { get; set; } = 0;
        public bool IsPremium { get; set; }
        public long PremiumUntil { get; set; }
        public string Status { get; set; } = "active";
    }
}