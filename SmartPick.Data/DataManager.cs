using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using SmartPick.Core.Interfaces;
using SmartPick.Core.Models;
using SmartPick.Data.CbTote;

namespace SmartPick.Data
{
    public class DataManager : IDataManager
    {
        private readonly IDistributedCache _cache;
        private readonly IServiceScopeFactory _scopeFactory;
        private const string CacheKeyPrefix = "smart-pick-pool";

        public Func<CbToteContext> CbContextFactory
        {
            get
            {
                using (var scope = _scopeFactory.CreateScope())
                {
                    var contextFactory = scope.ServiceProvider.GetRequiredService<Func<CbToteContext>>();
                    return contextFactory;
                }
            }
        }

        public DataManager(IDistributedCache cache, IServiceScopeFactory scopeFactory)
        {
            _cache = cache;
            _scopeFactory = scopeFactory;
        }

        public async Task FlushCachedPoolForLegAsync(int legId)
        {
            using (var context = CbContextFactory())
            {
                var leg = context.Legs.Find(legId);
                var pool = context.Pools.Find(leg.PoolId);
                if (pool.Status != "OPEN") return;

                var poolId = pool.Id;
                var cacheKey = $"{CacheKeyPrefix}-{poolId}";
                await _cache.RemoveAsync(cacheKey);
            }
        }

        public async Task<PoolModel> TryGetPoolModelAsync(int poolId, Dictionary<string, double> targetEvs, bool useCaching)
        {
            if (useCaching)
            {
                var cacheKey = $"{CacheKeyPrefix}-{poolId}";
                var cachedJson = await _cache.GetStringAsync(cacheKey);

                var poolModel = string.IsNullOrEmpty(cachedJson)
                    ? await BuildPoolModel(poolId, targetEvs)
                    : JsonConvert.DeserializeObject<PoolModel>(cachedJson);

                if (!string.IsNullOrEmpty(cachedJson))
                {
                    await _cache.RefreshAsync(cacheKey);
                    return poolModel;
                }
                var options = new DistributedCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromMinutes(15));
                await _cache.SetStringAsync(cacheKey, JsonConvert.SerializeObject(poolModel), options);

                return poolModel;
            }
            else
            {
                var poolModel = await BuildPoolModel(poolId, targetEvs);
                return poolModel;
            }
        }

        private async Task<PoolModel> BuildPoolModel(int poolId, Dictionary<string, double> targetEvs)
        {
            using(var context = CbContextFactory())
            {
                var pool = await context.Pools.Include(p => p.Legs).ThenInclude(l => l.Selections)
                    .FirstOrDefaultAsync(p => p.Id == poolId);
                if (pool == null) throw new InvalidOperationException($"Pool {poolId} Not Found");

                var legs = pool.Legs.OrderBy(l => l.LegOrder)
                    .Select(l => new LegModel(l.LegOrder,
                        l.Selections.Select(s => new SelectionModel(s.Bin,
                            pool.TypeCode == "RACE_PLACE"
                                ? (double) (s.PlaceProbability ?? 0)
                                : (double) s.Probability)).ToArray())).ToArray();

                var targetEv = targetEvs.ContainsKey(pool.TypeCode)? targetEvs[pool.TypeCode] : targetEvs["DEFAULT"];
                var poolModel = new PoolModel(pool.TypeCode, pool.HeadlinePrize, targetEv, legs);
                return poolModel;
            }
        }
    }
}
