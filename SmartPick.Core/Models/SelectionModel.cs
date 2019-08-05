using System;

namespace SmartPick.Core.Models
{
    [Serializable]
    public class SelectionModel : IEquatable<SelectionModel>
    {
        public int Bin { get; }
        public double Probability { get; private set; }

        public SelectionModel(int bin, double probability)
        {
            Bin = bin;
            Probability = probability;
        }

        public void UpdateProbability(double probability)
        {
            Probability = probability;
        }

        public bool Equals(SelectionModel other)
        {
            return Bin == other.Bin;
        }

        public override int GetHashCode()
        {
            return Bin;
        }
    }
}