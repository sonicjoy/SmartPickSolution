using NUnit.Framework;
using SmartPick.Core.Models;

namespace SmartPick.Tests.Models
{
    [TestFixture]
    public class PatternTest
    {
        [Test]
        public void PatternConstructorTest()
        {
            {
                var pattern = new Pattern("1/1/1/1");
                Assert.That(pattern.Count == 4);
                Assert.That(pattern[1] == 1);
            }

            {
                var pattern = new Pattern(string.Empty);
                Assert.That(pattern.Count == 0);
            }

            {
                var pattern = new Pattern("a//1,3/1/1");
                Assert.That(pattern.Count == 5);
                Assert.That(pattern[1] == 1);
                Assert.That(pattern[2] == 1);
                Assert.That(pattern[3] == 1);
            }
        }
    }
}