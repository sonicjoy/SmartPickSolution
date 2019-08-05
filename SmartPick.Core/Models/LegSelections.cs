using System;
using System.Collections.Generic;
using System.Linq;

namespace SmartPick.Core.Models
{
    public class LegSelections : HashSet<SelectionModel>
    {
        public readonly int Capacity;

        public IEnumerable<int> Bins => this.OrderBy(b => b.Bin).Select(s => s.Bin);
        public virtual double Probability => this.Sum(s => s.Probability);

        public LegSelections()
        {
        }

        public LegSelections(int capacity) : base(capacity)
        {
            Capacity = capacity;
        }

        public new bool Add(SelectionModel selection)
        {
            if (Capacity > 0 && Count >= Capacity || !base.Add(selection)) return false;
            return true;

        }

        public bool AddRange(IEnumerable<SelectionModel> selections)
        {
            return selections.Aggregate(false, (current, sel) => current | Add(sel));
        }

        public override string ToString() => string.Join(",", Bins);

        public void RemoveMin()
        {
            Remove(this.OrderBy(s => s.Probability).First());
        }
    }
}