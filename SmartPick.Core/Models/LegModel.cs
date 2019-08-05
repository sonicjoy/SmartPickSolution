using System;
using System.Collections.Generic;
using System.Linq;

namespace SmartPick.Core.Models
{
    [Serializable]
    public class LegModel
    {
        public int LegOrder { get; }
        public SelectionModel[] Selections { get; }

        public LegModel(int legOrder, SelectionModel[] selections)
        {
            LegOrder = legOrder;
            Selections = selections;
        }
    }
}