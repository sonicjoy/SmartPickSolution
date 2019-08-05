using NUnit.Framework;
using SmartPick.Core;
using System;
using System.Buffers;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;
using SmartPick.Core.Models;
using SmartPick.Tests.TestHelpers;

namespace SmartPick.Tests
{
    [TestFixture]
    public class FectaSmartPickServiceTests
    {
        private FectaSmartPickService _service;
        private int _fectaValue = 5;
        [SetUp]
        public void SetUp()
        {
            var legs = TestDataGenerator.GenerateLegs(_fectaValue, 32);
            var pool = new PoolModel("RACE_ORDER", 10000, 0.5, legs);
            _service = new FectaSmartPickService(pool);
        }

        [Test, TestCase(1), TestCase(48), TestCase(720)]
        public void GenerateLineCollectionTest(int lines)
        {
            var lc = _service.GenerateLineCollection(lines);
            Assert.That(lc.Count == lines);
            //Assert.That(lc.Probability * 10000 / lines > 0.25);
            Assert.That(lc.All(l => l.BinArray.Length == _fectaValue));
            Assert.That(lc.All(l => l.BinArray.ToHashSet().Count == l.BinArray.Length));
            Assert.That(lc.ToHashSet().Count == lc.Count);
        }

        [Test, TestCase(48), TestCase(10000)]
        public void GenerateLineCollectionPerformanceTest(int lines)
        {
            var timer = new Stopwatch();
            timer.Start();
            var lc = _service.GenerateLineCollection(lines);
            timer.Stop();
            Assert.That(lc.Count == lines);
            Console.WriteLine($"It took {timer.Elapsed} to generate {lines} lines.");
        }

        [Test, TestCase(1), TestCase(2), TestCase(4), TestCase(8), TestCase(16), TestCase(48), TestCase(72),
         TestCase(720)]
        public void GeneratePatternedSelectionsTest(int lines)
        {
            var ps = _service.GeneratePatternedSelections(lines);
            Assert.That(ps.LineCount == lines, 
                $"Expecting line count {lines}, but getting {ps.LineCount}");
            for (var i = 1; i < ps.Count - 1; i++)
                Assert.That(ps[i].Count > 1
                    ? ps[i].All(s => ps[i + 1].Contains(s))
                    : ps[i].All(s => !ps[i + 1].Contains(s)));
            Console.WriteLine(ps);
        }

        [Test, TestCase(48), TestCase(8000)]
        public void GenerateFectaSelectionPerformanceTest(int lines)
        {
            var timer = new Stopwatch();
            timer.Start();
            var ps = _service.GeneratePatternedSelections(lines);
            timer.Stop();
            Assert.That(ps.LineCount == lines, 
                $"Expecting line count {lines}, but getting {ps.LineCount}");
            Console.WriteLine($"It took {timer.Elapsed} to generate {lines} lines.");
        }

    }
}
