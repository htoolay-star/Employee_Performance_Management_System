namespace EPMS.Shared.Models
{
    public class CacheSettings
    {
        public int DefaultExpirationMinutes { get; set; } = 30;
        public int SlidingExpirationMinutes { get; set; } = 10;
        public int AbsoluteExpirationHours { get; set; } = 2;
    }
}
