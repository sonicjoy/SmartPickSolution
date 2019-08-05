using SmartPick.Core.Models;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;

namespace SmartPick.Core.Helpers
{
    public static class PatternGenerator
    {
        public static int[] CreatePattern(int lines, int legs)
        {
            var result = new int[legs];
            var primes = MathExtension.DecomposeInteger(lines);
            primes.MapTo(result);
            return result.OrderByDescending(c  => c).ToArray();
        }

        public static int[] CreateFectaPattern(int lines, int legs)
        {
            var result = new int[legs];
            var primes = MathExtension.DecomposeInteger(lines);
            primes.MapTo(result);
            var fectaResult = result.OrderBy(c => c).ToArray();

            var addCount = 0;
            for (var i = 0; i < fectaResult.Length; i++)
            {
                if (fectaResult[i] == 1) continue;
                fectaResult[i] += addCount++; //add 0, 1, 2, 3, ... 
            }

            return fectaResult;
        }
        
        public static int[] CreateSmartPattern(int lines, IReadOnlyDictionary<int, int> legSelections)
        {
            var fistCount = legSelections.Values.First();
            if (legSelections.Values.All(c => c == fistCount))
                return CreatePattern(lines, legSelections.Count);

            var primes = MathExtension.DecomposeInteger(lines);
            return primes.AssignTo(legSelections);           
        }

        private static void MapTo(this IList<int> source, IList<int> target)
        {
            var j = 0;
            for (var i = 0; i < target.Count; i++)
            {
                target[i] = j < source.Count ? source[j++] : 1;
            }

            while (j < source.Count)
            {
                var min = target.Min();
                target[target.IndexOf(min)] *= source[j++];
            }
        }

        private static int[] AssignTo(this IList<int> source, IReadOnlyDictionary<int, int> legSelections)
        {
            var size = legSelections.Count;
            var weightedLegs = legSelections.ToDictionary(l => l.Key, l => new WeightedLeg { Weight = l.Value + (size - l.Key) * 2, SelectionCount = 1 });

            //O(p^l) time complexity
            foreach (var p in source)
            {
                var key = weightedLegs.Where(l => legSelections[l.Key] > l.Value.SelectionCount * p)
                    .OrderByDescending(l => l.Value.Weight)
                    .First().Key; //in case the available selections are less than the prime
                weightedLegs[key].Weight /= p;
                weightedLegs[key].SelectionCount *= p;
            }

            var result = new int[size];
            foreach (var leg in weightedLegs)
            {
                result[leg.Key - 1] = leg.Value.SelectionCount;
            }
            return result;
        }

        private class WeightedLeg
        {
            public double Weight;
            public int SelectionCount;
        }
    }
}
