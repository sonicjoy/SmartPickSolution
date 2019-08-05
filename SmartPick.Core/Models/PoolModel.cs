using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using SmartPick.Core.Helpers;

namespace SmartPick.Core.Models
{
    [Serializable]
    public class PoolModel
    {

        public string TypeCode { get; }
        public LegModel[] Legs { get; }
        public decimal Prize;
        public double TargetEv;
        public Dictionary<int, Pattern> SmartPickPatterns { get; }

        public PoolModel(string typeCode, decimal prize, double targetEv, LegModel[] legs)
        {
            TypeCode = typeCode;
            Prize = prize;
            TargetEv = targetEv;
            Legs = legs;
            SmartPickPatterns = new Dictionary<int, Pattern>();
        }

        public Pattern GetSmartPickPattern(int lines) => SmartPickPatterns.ContainsKey(lines)
            ? SmartPickPatterns[lines]
            : GenerateSmartPattern(lines);

        private Pattern GenerateSmartPattern(int lines)
        {
            var bins = TypeCode == "RACE_ORDER"
                ? PatternGenerator.CreateFectaPattern(lines, Legs.Length)
                : PatternGenerator.CreateSmartPattern(lines,
                    Legs.ToDictionary(l => l.LegOrder, l => l.Selections.Length));
            var pattern = new Pattern(bins.ToArray());
            SmartPickPatterns[lines] = pattern;
            return pattern;
        }

        public double GetTargetProbability(int lines) => lines * TargetEv / (double)Prize;

    }
}