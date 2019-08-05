using System.Collections.Generic;
using System.Linq;

namespace SmartPick.Core.Models.Fecta
{
    public class LineCollection : HashSet<Line>
    {
        private int _legs;

        public double Probability => this.Sum(l => l.Probability);
        
        public LineCollection(int legs)
        {
            _legs = legs;
        }

        public new bool Add(Line line)
        {
            return line.BinArray.Length == _legs && base.Add(line);
        }

        public override string ToString()
        {
            var result = new HashSet<int>[_legs];
            for (var i = 0; i < _legs; i++)
            {
                result[i] = new HashSet<int>(this.Select(l => l.BinArray[i]));
            }

            return string.Join("/", result.Select(l => string.Join(",", l)));
        }
    }
}