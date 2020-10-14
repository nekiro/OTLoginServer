namespace OTLoginServer.Models
{
    public class Account
    {
        public int Id { get; set; }
        public long PremiumUntil { get; set; }
        public long LastLoginTime { get; set; }
        public bool IsPremium { get; set; }
    }
}
