using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SmartPick.Core.Models.RacePlace;

namespace SmartPick.Core.Models
{
    public class PatternedSelections<T> : Dictionary<int, T> where T : LegSelections
    {
        public virtual double Probability =>
            Values.Aggregate(1d, (product, next) => product * next.Probability, product => Count > 0 ? product : 0);

        public virtual int LineCount => 
            Values.Aggregate(1, (product, next) => product * next.Count, product => Count > 0 ? product : 0);

        public PatternedSelections(IReadOnlyList<T> legs) : base(legs.Count)
        {
            var capacity = legs.Count;

            for (var legOrder = 1; legOrder <= capacity; legOrder++)
            {
                this[legOrder] = legs[legOrder-1];
            }
        }

        public override string ToString() => 
            string.Join("/", this.OrderBy(d => d.Key).Select(l => l.Value.ToString()));
    }
}
