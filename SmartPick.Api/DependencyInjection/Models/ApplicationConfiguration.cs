using System.Collections.Generic;

namespace SmartPick.Api.DependencyInjection.Models
{
    public class ApplicationConfiguration
    {
        public bool UseKafka { get; set; } = true;
        public bool UseCaching { get; set; } = true;
        public Dictionary<string, double> TargetEvs { get; set; }
    }
}
