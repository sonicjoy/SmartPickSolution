using System.Collections.Generic;
using System.Linq;

namespace SmartPick.Core.Models
{
    public class Pattern : Dictionary<int, int>
    {
        public Pattern(string pattern)
        {
            if (string.IsNullOrEmpty(pattern)) return;

            var legOrder = 1;
            foreach (var str in pattern.Split("/").Select(l => l))
            {
                this[legOrder] = int.TryParse(str, out var ct) ? ct : 1;
                legOrder++;
            }
        }

        public Pattern(IReadOnlyCollection<int> pattern)
        {
            if (pattern == null || !pattern.Any()) return;

            var legOrder = 1;
            foreach (var i in pattern)
            {
                this[legOrder] = i;
                legOrder++;
            }
        }
    }
}