using NUnit.Framework;
using SmartPick.Core.Models;
using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestPlatform.Common.Utilities;

namespace SmartPick.Tests.Models
{
    [TestFixture]
    public class PatternedSelectionsTest
    {
        [Test]
        public void EmptyResultTest()
        {
            var legs = new List<LegSelections>();
            var patternedSel = new PatternedSelections<LegSelections>(legs);
            Assert.That(patternedSel.Probability == 0d);
            Assert.That(patternedSel.LineCount == 0);
        }

        [Test]
        public void ProbabilityAggregationTest()
        {
            var legs = new List<LegSelections>
            {
                new LegSelections(3),
                new LegSelections(3),
                new LegSelections(3),
            };
            var patternedSel = new PatternedSelections<LegSelections>(legs);

            patternedSel[1].Add(new SelectionModel(1, 0.2));
            patternedSel[1].Add(new SelectionModel(2, 0.2));
            patternedSel[1].Add(new SelectionModel(3, 0.2));

            patternedSel[2].Add(new SelectionModel(1, 0.1));
            patternedSel[2].Add(new SelectionModel(2, 0.2));
            patternedSel[2].Add(new SelectionModel(3, 0.3));

            patternedSel[3].Add(new SelectionModel(1, 0.1));
            patternedSel[3].Add(new SelectionModel(2, 0.1));
            patternedSel[3].Add(new SelectionModel(3, 0.1));

            Assert.That(Math.Abs(patternedSel.Probability-0.108) < 0.00000001);
            Assert.That(patternedSel.LineCount == 27);
        }
    }
}