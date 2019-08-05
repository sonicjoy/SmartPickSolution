using NUnit.Framework;
using SmartPick.Core;
using System;
using System.Buffers;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SmartPick.Core.Models;
using SmartPick.Tests.TestHelpers;

namespace SmartPick.Tests
{
    [TestFixture]
    public class RacePlaceSmartPickServiceTests
    {
        private RacePlaceSmartPickService _service;
        private PoolModel _pool;
        private decimal _prize = 10000m;
        private double _targetEv = 0.5;
        [SetUp]
        public void SetUp()
        {
            var legs = TestDataGenerator.FixedPlace6LegModels();
            _pool = new PoolModel("RACE_PLACE", _prize, _targetEv, legs);
            _service = new RacePlaceSmartPickService(_pool);
        }

        [Test, TestCase(1), TestCase(48), TestCase(72), TestCase(240), TestCase(360)]
        public void GenerateLegSelectionsTest(int lines)
        {
            var result = _service.GeneratePatternedSelections(lines);
            Assert.That(result.LineCount == lines);
            var evRatio = result.Probability * (double) _prize / lines;
            Assert.That(evRatio > _targetEv, 
                $"Expecting ev ratio over {_targetEv * 100}%, but getting only {evRatio * 100}%");
        }

        [Test, TestCase(1), TestCase(48), TestCase(720)]
        public void GetTargetProbabilityTest(int lines)
        {
            var result = _pool.GetTargetProbability(lines);
            var lowerLimit = lines * _targetEv / (double)_prize;
            var higherLimit = _pool.Legs.Aggregate(1d, (prd, l) => prd * l.Selections.Sum(s => s.Probability));
            Assert.That(result >= lowerLimit);
            Assert.That(result < higherLimit);
        }
    }
}
