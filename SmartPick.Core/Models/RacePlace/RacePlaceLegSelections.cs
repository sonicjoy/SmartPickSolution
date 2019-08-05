using System;
using System.Linq;

namespace SmartPick.Core.Models.RacePlace
{
    public class RacePlaceLegSelections : LegSelections
    {
        private readonly int _notPlaced;

        public override double Probability
        {
            get
            {
                var product = 1d;

                if (Count > _notPlaced)
                    return 1;

                foreach (var selection in this)
                {
                    product *= 1 - selection.Probability;
                }


                return 1 - product;
            }
        }

        public RacePlaceLegSelections(int capacity, int notPlaced) : base(capacity)
        {
            _notPlaced = notPlaced;
        }
    }
}
