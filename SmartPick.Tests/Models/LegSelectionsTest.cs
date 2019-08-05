using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using SmartPick.Core.Models;

namespace SmartPick.Tests.Models
{
    [TestFixture]
    public class LegSelectionsTest
    {
        private const double Tolerance = 0.00000001;
        [Test]
        public void LimitedCapacityTest()
        {
            var legSel = new LegSelections(3);
            Assert.That(legSel.Add(new SelectionModel(1, 0.3)));
            Assert.That(legSel.Add(new SelectionModel(2, 0.3)));
            Assert.That(legSel.Add(new SelectionModel(3, 0.3)));

            Assert.That(!legSel.Add(new SelectionModel(4, 0.3)));
        }

        [Test]
        public void LimitedCapacityAddRangeTest()
        {
            var legSel = new LegSelections(3);
            Assert.That(legSel.AddRange(new List<SelectionModel>
            {
                new SelectionModel(1, 0.3), 
                new SelectionModel(2, 0.3),
                new SelectionModel(3, 0.3),
                new SelectionModel(4, 0.3)
            }));

            Assert.That(legSel.Count == 3);
        }

        [Test]
        public void BinsAndProbabilityTest()
        {
            var legSel = new LegSelections(3);
            Assert.That(legSel.Add(new SelectionModel(1, 0.3)));
            Assert.That(legSel.Add(new SelectionModel(2, 0.3)));
            Assert.That(legSel.Add(new SelectionModel(3, 0.3)));

            Assert.That(legSel.Bins.Count() == 3);
            Assert.That(Math.Abs(legSel.Probability - 0.9d) < Tolerance);
        }

        [Test]
        public void RemoveMinTest()
        {
            var legSel = new LegSelections(3)
            {
                new SelectionModel(1, 0.4),
                new SelectionModel(2, 0.3),
                new SelectionModel(3, 0.2)
            };

            legSel.RemoveMin();

            Assert.That(legSel.Count == 2);
            Assert.That(legSel.Min(s => s.Probability) > 0.2);
        }
    }
}