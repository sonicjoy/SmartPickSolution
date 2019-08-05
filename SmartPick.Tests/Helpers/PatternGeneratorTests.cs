using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using SmartPick.Core.Helpers;

namespace SmartPick.Tests.Helpers
{
    [TestFixture]
    public class PatternGeneratorTests
    {
        [Test]
        public void PlainPatternTest()
        {
            var pattern = PatternGenerator.CreatePattern(48, 5);
            Assert.That(pattern.Length == 5);
            Assert.That(pattern.Aggregate(1, (p, next) =>p * next) == 48);
        }

        [Test, TestCase(2), TestCase(4), TestCase(8), TestCase(16), TestCase(24), TestCase(48), TestCase(72), TestCase(120), TestCase(180), TestCase(240)]
        public void FectaPatternTest(int lines)
        {
            var pattern = PatternGenerator.CreateFectaPattern(lines, 4);
            Assert.That(pattern.Length == 4);
            Console.WriteLine(string.Join("/", pattern));          
        }

        [Test, TestCase(2), TestCase(4), TestCase(8), TestCase(16), TestCase(24), TestCase(48), TestCase(72), TestCase(120), TestCase(180), TestCase(240), TestCase(720)]
        public void SmartPatternTest(int lines)
        {
            var legSelections = new Dictionary<int, int>
            {
                [1] = 32,
                [2] = 12,
                [3] = 21,
                [4] = 6,
                [5] = 27,
                [6] = 40
            };

            var pattern = PatternGenerator.CreateSmartPattern(lines, legSelections);
            Assert.That(pattern.Length == 6);
            Assert.That(pattern.Aggregate(1, (p, next) =>p * next) == lines);
            Assert.That(pattern[5] == pattern.Max() || pattern[0] == pattern.Max());
            Assert.That(pattern[3] == pattern.Min());
            Console.WriteLine(string.Join("/", pattern)); 
        }

        [Test, TestCase(2), TestCase(4), TestCase(8), TestCase(16), TestCase(24), TestCase(48), TestCase(72), TestCase(120), TestCase(180), TestCase(240), TestCase(720)]
        public void SmartPatternForEqualLegsTest(int lines)
        {
            var legSelections = new Dictionary<int, int>
            {
                [1] = 17,
                [2] = 17,
                [3] = 17,
                [4] = 17,
                [5] = 17,
                [6] = 17
            };

            var pattern = PatternGenerator.CreateSmartPattern(lines, legSelections);
            Assert.That(pattern.Length == 6);
            Assert.That(pattern.Aggregate(1, (p, next) =>p * next) == lines);
            Assert.That(pattern[0] == pattern.Max());
            Assert.That(pattern[5] == pattern.Min());
            Console.WriteLine(string.Join("/", pattern)); 
        }
    }
}