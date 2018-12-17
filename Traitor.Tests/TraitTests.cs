namespace Traitor.Tests
{
    using System;
    using NUnit.Framework;

    [TestFixture]
    public class TraitTests
    {
        [Test]
        public void AssignKey_KeyIsEqual()
        {
            var t = new Trait<string, int>("foo", (TraitValue<int>)9);

            Assert.That(t.Key, Is.EqualTo("foo"));
        }

        [Test]
        public void AssignValue_ValueIsEqual()
        {
            var t = new Trait<string, int>("foo", (TraitValue<int>)9);

            Assert.That(t.Value, Is.EqualTo((TraitValue<int>)9));
        }
    }
}
