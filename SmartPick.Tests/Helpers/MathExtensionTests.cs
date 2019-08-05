using System.Linq;
using NUnit.Framework;
using SmartPick.Core.Helpers;

namespace SmartPick.Tests.Helpers
{
    [TestFixture]
    public class MathExtensionTests
    {
        [Test]
        public void DecomposeNumberTest()
        {
            var result = MathExtension.DecomposeInteger(48);
            Assert.That(result.ToList().TrueForAll(MathExtension.IsPrime));
            Assert.That(result.Aggregate(1, (p, next) =>p * next) == 48);
        }
    }
}