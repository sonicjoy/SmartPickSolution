using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SmartPick.Core.Models;

namespace SmartPick.Core.Helpers
{
    public class SmartPickHelper
    {            
        public static IEnumerable<SelectionModel> GetGoodSelections(IEnumerable<SelectionModel> selections, int takes)
        {
            if (takes == 0) return selections.Take(takes);
            if (!selections.Any() || selections.Count() <= takes) return selections;

            var rand = new Random();
            var skips = rand.Next(0, selections.Count() - takes);
            return selections //this will skip some random bad selections and then from the result set take random selections
                .OrderBy(s => s.Probability)
                .Skip(skips)
                .TakeRandom(takes);
        }
    }
}
