using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SmartPick.Api.DependencyInjection.Models;
using SmartPick.Api.Interfaces;
using SmartPick.Api.Models;
using SmartPick.Core.Interfaces;

namespace SmartPick.Api.Controllers
{
    [Route("pools")]
    [ApiController]
    public class PoolController : ControllerBase
    {
        private readonly ILogger<PoolController> _logger;
        private readonly ISmartPickServiceFactory _serviceFactory;
        private readonly ApplicationConfiguration _config;

        public PoolController(ILogger<PoolController> logger, ISmartPickServiceFactory smartPickServiceFactory,
            ApplicationConfiguration config)
        {
            _logger = logger;
            _serviceFactory = smartPickServiceFactory;
            _config = config;
        }

        /// <summary>
        /// End point for testing
        /// </summary>
        /// <returns></returns>
        public ActionResult<string> Get()
        {
            _logger.LogInformation("Test end point called");
            var result = new SelectionResult {Selections = "7,13/2,15/17/2"};
            return $"Example request GET pools/<pool_id>/smart_pick?lines=4, response: {result}";
        }

        /// <summary>
        /// Get smart pick selections
        /// </summary>
        /// <param name="poolId">A valid pool id.</param>
        /// <param name="lines">Number of permutations requested.</param>
        /// <returns></returns>
        [HttpGet("{poolId}/smart_pick")]
        public async Task<ActionResult<ISelectionResult>> GetSmartPickAsync(int poolId, int lines)
        {
            var service = await _serviceFactory.GetSmartPickService(poolId, _config.TargetEvs, _config.UseCaching);
            var selections = new SelectionResult {Selections = service.GetSelections(lines)};
            return selections;
        }
    }
}
