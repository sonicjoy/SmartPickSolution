namespace SmartPick.Api.DependencyInjection.Models
{
    public class RedisConfiguration
    {
        public string MasterName { get; set; }
        public EndPoint[] SentinelServers { get; set; }
        public string Password { get; set; }
    }
}
