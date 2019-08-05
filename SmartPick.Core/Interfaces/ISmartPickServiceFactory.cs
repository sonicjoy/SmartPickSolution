using System.Collections.Generic;
using System.Threading.Tasks;
using SmartPick.Core.Models;

namespace SmartPick.Core.Interfaces
{
    public interface ISmartPickServiceFactory
    {
        Task<ISmartPickService> GetSmartPickService(int poolId, Dictionary<string, double> targetEvs, bool useCaching);
    }
}