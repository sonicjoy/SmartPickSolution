using System.Collections.Generic;

namespace SmartPick.Core.Models.Fecta
{
    public class FectaPatternedSelections : PatternedSelections<LegSelections>
    {
        public override int LineCount 
        {
            get
            {
                var product = 1;
                var subCount = 0;

                foreach (var legSelections in Values)
                {
                    if (legSelections.Count == 1) continue;
                    product *= legSelections.Count - subCount++;
                }

                return product;
            }
        }

        public FectaPatternedSelections(IReadOnlyList<LegSelections> legs) : base(legs)
        {
        }
    }
}
