using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using SmartPick.Core.Helpers;
using SmartPick.Core.Interfaces;
using SmartPick.Core.Models;
using SmartPick.Core.Models.Fecta;

namespace SmartPick.Core
{
    public class FectaSmartPickService : SmartPickService<LegSelections>
    {
        public FectaSmartPickService(PoolModel pool) : base(pool)
        {
        }

        protected override PatternedSelections<LegSelections> GetPatternedSelections(IReadOnlyList<LegSelections> legs)
        {
            var result = new FectaPatternedSelections(legs);
            var usedBins = new HashSet<int>();
            foreach (var leg in Pool.Legs) //Legs should be prepared in leg order when building the Pool model
            {
                var legOrder = leg.LegOrder;
                var selCnt = result[legOrder].Capacity;
                var validSelections = leg.Selections.Where(s => s.Probability > 0).ToArray();
                if (selCnt > 2 && legOrder > 1)
                {
                    var binsInPreviousLeg = result[legOrder - 1].Bins;
                    result[legOrder].AddRange(validSelections.Where(s => binsInPreviousLeg.Contains(s.Bin)));
                }

                var additionalSelections = SmartPickHelper.GetGoodSelections(
                    validSelections.Where(s => !usedBins.Contains(s.Bin)), selCnt - result[legOrder].Count).ToArray();

                result[legOrder].AddRange(additionalSelections);

                additionalSelections.ToList().ForEach(sel => { usedBins.Add(sel.Bin); });
            }

            return result;
        }

        protected override void ImproveSelections(PatternedSelections<LegSelections> result, double targetProbs)
        {
            //todo: need a algorithm and DO NOT REMOVE this override method as it will fall back to the default one
        }

        [Obsolete]
        public LineCollection GenerateLineCollection(int lines)
        {
            var result = new LineCollection(Pool.Legs.Length);
            if (lines <= 0) return result;

            while (result.Count < lines)
            {
                var line = GenerateNewLine();
                result.Add(line);
            }

            return result;
        }

        private Line GenerateNewLine()
        {
            var line = new int[Pool.Legs.Length];
            var probs = 1d;
            foreach (var leg in Pool.Legs)
            {
                var legOrder = leg.LegOrder;
                var legSels = leg.Selections.Where(s => !line.Contains(s.Bin));
                var sel = SmartPickHelper.GetGoodSelections(legSels, 1).First();
                line[legOrder - 1] = sel.Bin;
                probs *= sel.Probability;
            }

            return new Line(line, probs);
        }
        

    }
}
