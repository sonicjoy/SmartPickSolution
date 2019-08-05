using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SmartPick.Core.Models;

namespace SmartPick.Core.Interfaces
{
    public interface IDataManager
    {
        Task<PoolModel> TryGetPoolModelAsync(int poolId, Dictionary<string, double> targetEvs, bool useCaching);
        Task FlushCachedPoolForLegAsync(int legId);
    }
}