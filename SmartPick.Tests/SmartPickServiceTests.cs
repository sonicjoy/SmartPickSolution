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
    public class SmartPickServiceTests
    {
        private SmartPickService<LegSelections> _service;
        private PoolModel _pool;
        private decimal _prize = 10000m;
        private double _targetEv = 0.5;
        [SetUp]
        public void SetUp()
        {
            var legs = TestDataGenerator.GenerateLegs(10, 2);
            _pool = new PoolModel("BTTS", _prize, _targetEv, legs);
            _service = new SmartPickService<LegSelections>(_pool);
        }

        [Test, TestCase(1), TestCase(16), TestCase(128)]
        public void GenerateLegSelectionsTest(int lines)
        {
            var result = _service.GeneratePatternedSelections(lines);
            Assert.That(result.LineCount == lines);
            Assert.That(result[1].Count >= result[2].Count);
            var evRatio = result.Probability * (double) _prize / lines;
            Assert.That(evRatio > _targetEv, 
                $"Expecting ev ratio over {_targetEv * 100}%, but getting only {evRatio * 100}%");
        }

        [Test, TestCase(1), TestCase(16), TestCase(128)]
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
