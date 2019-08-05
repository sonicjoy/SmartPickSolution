using System.Linq;
using System.Threading.Tasks;
using KafkaConnector.Helpers;
using KafkaConnector.Models;
using Microsoft.Extensions.Logging;
using SmartPick.Core.Interfaces;
using SmartPick.Data;

namespace SmartPick.Api.DependencyInjection.Messaging
{
    public class SelectionListener : KafkaListenerTemplate
    {
        private readonly ILogger _logger;
        private readonly IDataManager _data;

        public static readonly string topic = "db_master.cbtote.selections";

        public SelectionListener(ILogger<SelectionListener> logger, IDataManager dataManager) : base(topic)
        {
            _logger = logger;
            _data = dataManager;
        }

        protected override void ProcessMessage(dynamic message, long offset)
        {
            _logger.LogDebug($"Selection listener starts processing message: {message}, offset: {offset}");
            var msg = (DifferenceMessage) message;
            if (msg == null) return;

            var before = msg.Before;
            var after = msg.After;
            var diffs = msg.GetDifferences();

            if (!diffs.Contains("probability") && !diffs.Contains("place_probability") || after == null || !after.ContainsKey("leg_id")) return;

            var legId = (int) after["leg_id"];
            _logger.LogInformation($"Probabilities updated for leg {legId}");
            _ = Task.Run(() => _data.FlushCachedPoolForLegAsync(legId));
            _logger.LogInformation($"Flushed pool cache for leg {legId}");
        }
    }
}
