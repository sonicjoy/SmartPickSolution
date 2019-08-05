using System.Collections.Generic;
using System.Linq;
using SmartPick.Core.Helpers;
using SmartPick.Core.Interfaces;
using SmartPick.Core.Models;

namespace SmartPick.Core
{
    public class SmartPickService<T> : ISmartPickService where T : LegSelections
    {
        protected PoolModel Pool { get; }

        public SmartPickService(PoolModel pool)
        {
            Pool = pool;
        }

        public string GetSelections(int lines) => GeneratePatternedSelections(lines).ToString();

        public PatternedSelections<T> GeneratePatternedSelections(int lines)
        {
            var pattern = Pool.GetSmartPickPattern(lines);
            var legs = GetLegPatterns(pattern);
            var result = GetPatternedSelections(legs);
            var targetProbs = Pool.GetTargetProbability(lines);
            ImproveSelections(result, targetProbs);

            return result;
        }

        protected virtual PatternedSelections<T> GetPatternedSelections(IReadOnlyList<T> legs)
        {
            var result = new PatternedSelections<T>(legs);
            foreach (var leg in Pool.Legs)
            {
                var legOrder = leg.LegOrder;
                var availableSelections = leg.Selections.Where(s => s.Probability > 0);
                var selCnt = result[legOrder].Capacity;
                var additionalSelections = SmartPickHelper.GetGoodSelections(availableSelections, selCnt);
                result[legOrder].AddRange(additionalSelections);
            }

            return result;
        }

        protected virtual IReadOnlyList<T> GetLegPatterns(Pattern pattern)
        {
            var legs = new List<LegSelections>();
            for (var legOrder = 1; legOrder <= Pool.Legs.Length; legOrder++)
            {
                var leg = new LegSelections(pattern[legOrder]);
                legs.Add(leg);
            }

            return (IReadOnlyList<T>)legs;
        }

        protected virtual void ImproveSelections(PatternedSelections<T> result, double targetProbs)
        {
            const int trials = 1000; //this is equivalent to set the longest waiting time
            var legInd = 0;
            while (result.Probability < targetProbs && legInd < trials)
            {
                var legOrder = legInd % Pool.Legs.Length + 1;
                var minProb = result[legOrder].Min(b => b.Probability);
                var availableSelection = Pool.Legs[legOrder - 1].Selections
                    .Where(s => s.Probability > minProb && !result[legOrder].Contains(s)).OrderBy(s => s.Probability)
                    .FirstOrDefault();
                if(availableSelection != default(SelectionModel))
                {
                    result[legOrder].RemoveMin();
                    result[legOrder].Add(availableSelection);
                }
                legInd++;
            }
        }
    }
}
