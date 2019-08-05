using System;
using System.Linq;

namespace SmartPick.Core.Models.Fecta
{
    public struct Line : IEquatable<Line>
    {
        public int[] BinArray { get; }
        public double Probability { get; }

        public Line(int[] binArray, double probability)
        {
            BinArray = binArray;
            Probability = probability;
        }

        public bool Equals(Line other)
        {
            return BinArray.SequenceEqual(other.BinArray);
        }

        public override int GetHashCode()
        {
            return BinArray.Aggregate(BinArray.Length, (current, t) => unchecked(current * 31 + t));
        }

        public override string ToString()
        {
            return $"{string.Join('/', BinArray)}";
        }
    }
}