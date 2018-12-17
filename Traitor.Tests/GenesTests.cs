namespace Traitor.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using NUnit.Framework;

    [TestFixture(TestOf = typeof(Genes<,>))]
    public class GenesTests
    {
        [Test]
        public void Ctor_Null_ThrowsArgumentNullException()
        {
            Assert.That(() => new Genes<string, int>(null), Throws.ArgumentNullException);
        }

        [Test]
        public void ContainsKey_HasTrait_ReturnsTrue()
        {
            var genes = new Genes<string, int>(new[] { new Trait<string, int>("foo", 9) });

            Assert.That(genes.ContainsKey("foo"), Is.True);
        }

        [Test]
        public void ContainsKey_DoesNotHaveTrait_ReturnsFalse()
        {
            var genes = new Genes<string, int>(new[] { new Trait<string, int>("foo", 9) });

            Assert.That(genes.ContainsKey("bar"), Is.False);
        }

        [Test]
        public void TryGet_HasTrait_ReturnsTrue()
        {
            var genes = new Genes<string, int>(new[] { new Trait<string, int>("foo", 9) });

            Assert.That(genes.TryGet("foo", out var res), Is.True);
        }

        [Test]
        public void TryGet_DoesNotHaveTrait_ReturnsFalse()
        {
            var genes = new Genes<string, int>(new[] { new Trait<string, int>("foo", 9) });

            Assert.That(genes.TryGet("bar", out var res), Is.False);
        }

        [Test]
        public void TryGet_HasTrait_ValueIsCorrect()
        {
            var genes = new Genes<string, int>(new[] { new Trait<string, int>("foo", 9) });
            genes.TryGet("foo", out var res);

            Assert.That(res, Is.EqualTo((TraitValue<int>)9));
        }

        [Test]
        public void GetKeys_ReturnsAllKeys()
        {
            var genes = new Genes<string, int>(new[] { new Trait<string, int>("foo", 9) });
            var keys = genes.GetKeys();

            Assert.That(keys, Is.EquivalentTo(new[] { "foo" }));
        }

        [Test]
        public void Count_SingleTrait_ReturnsOne()
        {
            var genes = new Genes<string, int>(new[] { new Trait<string, int>("foo", 9) });

            Assert.That(genes.Count, Is.EqualTo(1));
        }

        [Test]
        public void Count_Empty_ReturnsCount()
        {
            var genes = Genes<string, int>.Empty;

            Assert.That(genes.Count, Is.Zero);
        }

        [Test]
        public void Indexer_IndexMinusOne_ThrowsArgumentOutOfRangeException()
        {
            var genes = Genes<string, int>.Empty;

            Assert.That(() => genes[-1], Throws.InstanceOf<ArgumentOutOfRangeException>());
        }

        [Test]
        public void Indexer_IndexPlusOne_ThrowsArgumentOutOfRangeException()
        {
            var genes = new Genes<string, int>(new[] { new Trait<string, int>("foo", 123) } );

            Assert.That(() => genes[1], Throws.InstanceOf<ArgumentOutOfRangeException>());
        }

        [Test]
        public void Indexer_IndexZero_ReturnsTrait()
        {
            var genes = new Genes<string, int>(new[] { new Trait<string, int>("foo", 123) });

            Assert.That(genes[0], Is.EqualTo(new Trait<string, int>("foo", 123)));
        }
    }
}
