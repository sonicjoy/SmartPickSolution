using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SmartPick.Core.Helpers;
using SmartPick.Core.Models;
using SmartPick.Core.Models.RacePlace;

namespace SmartPick.Core
{
    public class RacePlaceSmartPickService : SmartPickService<RacePlaceLegSelections>
    {
        public RacePlaceSmartPickService(PoolModel pool) : base(pool)
        {
        }

        protected override IReadOnlyList<RacePlaceLegSelections> GetLegPatterns(Pattern pattern)
        {
            var legs = new List<RacePlaceLegSelections>();
            for (var legOrder = 1; legOrder <= Pool.Legs.Length; legOrder++)
            {
                var places = GetPlacesFromProbability(Pool.Legs[legOrder - 1].Selections);
                var notPlaced = Pool.Legs[legOrder - 1].Selections.Count(s => s.Probability > 0) - places; 
                var legSelections = new RacePlaceLegSelections(pattern[legOrder], notPlaced);
                legs.Add(legSelections);
            }

            return legs;
        }

        private int GetPlacesFromProbability(IEnumerable<SelectionModel> selections)
        {
            var sumOfProbs = selections.Sum(s => s.Probability);
            var places = (int) Math.Round(sumOfProbs, MidpointRounding.AwayFromZero);

            return places;
        }
    }
}
