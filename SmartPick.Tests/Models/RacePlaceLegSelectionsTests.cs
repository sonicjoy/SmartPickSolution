using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using NUnit.Framework.Internal;
using SmartPick.Core.Models;
using SmartPick.Core.Models.RacePlace;

namespace SmartPick.Tests.Models
{
    [TestFixture]
    public class RacePlaceLegSelectionsTests
    {
        private const double Tolerance = 0.00000001;

        [Test]
        public void ZeroProbabilityTest()
        {
            var leg = new RacePlaceLegSelections(5, 3);
            Assert.That(Math.Abs(leg.Probability) < Tolerance, 
                $"Expecting 0 probability for the leg without any selections, but we get {leg.Probability}");
        }

        [Test]
        public void OneProbabilityTest()
        {
            var leg = new RacePlaceLegSelections(5, 3);
            leg.AddRange(new List<SelectionModel>
            {
                new SelectionModel(1, 0.4d),
                new SelectionModel(2, 0.4d),
                new SelectionModel(3, 0.4d),
                new SelectionModel(4, 0.4d),
                new SelectionModel(5, 0.4d),
            });
            Assert.That(Math.Abs(leg.Probability - 1) < Tolerance, 
                $"Expecting 100% probability for the leg with all selections, but we get {leg.Probability}");
        }

        [Test]
        public void TwoSelectionProbabilityTest()
        {
            var leg = new RacePlaceLegSelections(2, 3);
            leg.AddRange(new List<SelectionModel>
            {
                new SelectionModel(1, 0.4d),
                new SelectionModel(2, 0.4d),
            });
            Assert.That(Math.Abs(leg.Probability - 0.64) < Tolerance, 
                $"Expecting 64% probability for the leg with 2 selections, but we get {leg.Probability}");
        }

        [Test]
        public void ThreeSelectionProbabilityTest()
        {
            var leg = new RacePlaceLegSelections(3, 3);
            leg.AddRange(new List<SelectionModel>
            {
                new SelectionModel(1, 0.4d),
                new SelectionModel(2, 0.4d),
                new SelectionModel(3, 0.4d),
            });
            Assert.That(Math.Abs(leg.Probability - 0.784) < Tolerance, 
                $"Expecting 78.4% probability for the leg with 3 selections, but we get {leg.Probability}");
        }
    }
}
