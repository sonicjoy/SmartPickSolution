using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using SmartPick.Core.Interfaces;
using SmartPick.Core.Models;

namespace SmartPick.Core
{
    public class SmartPickServiceFactory : ISmartPickServiceFactory
    {
        private readonly IDataManager _dataManager;

        public SmartPickServiceFactory(IDataManager dataManager)
        {
            _dataManager = dataManager;
        }

        public async Task<ISmartPickService> GetSmartPickService(int poolId, Dictionary<string, double> targetEvs, bool useCaching)
        {
            var pool = await _dataManager.TryGetPoolModelAsync(poolId, targetEvs, useCaching);
            switch (pool.TypeCode)
            {
                case "RACE_ORDER": 
                    return new FectaSmartPickService(pool);
                case "RACE_PLACE":
                    return new RacePlaceSmartPickService(pool);
                default:
                    return new SmartPickService<LegSelections>(pool);
            }
        }
    }
}
