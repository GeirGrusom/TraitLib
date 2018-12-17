namespace Traitor.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using NUnit.Framework;

    [TestFixture(TestOf = typeof(TraitValue<>))]
    public class TraitValueTests
    {
        [Test]
        public void OperatorInt_CreatesTraitWithValue()
        {
            var traitValue = (TraitValue<int>)9;

            Assert.That((int)traitValue, Is.EqualTo(9));
        }

        [Test]
        public void OperatorEquals_ValuesAreEqual_ReturnsTrue()
        {
            var traitValueA = (TraitValue<int>)9;
            var traitValueB = (TraitValue<int>)9;

            var result = traitValueA == traitValueB;

            Assert.That(result, Is.True);
        }

        [Test]
        public void OperatorEquals_ValuesAreNotEqual_ReturnsFalse()
        {
            var traitValueA = (TraitValue<int>)9;
            var traitValueB = (TraitValue<int>)18;

            var result = traitValueA == traitValueB;

            Assert.That(result, Is.False);
        }

        [Test]
        public void Equals_ValuesAreEqual_ReturnsTrue()
        {
            var traitValueA = (TraitValue<int>)9;
            var traitValueB = (TraitValue<int>)9;

            Assert.That(traitValueA.Equals(traitValueB), Is.True);
        }

        [Test]
        public void Equals_ValuesAreNotEqual_ReturnsFalse()
        {
            var traitValueA = (TraitValue<int>)9;
            var traitValueB = (TraitValue<int>)18;

            Assert.That(traitValueA.Equals(traitValueB), Is.False);
        }

        [Test]
        public void Equals_Null_ReturnsFalse()
        {
            var traitValue = (TraitValue<int>)9;

            Assert.That(traitValue.Equals(null), Is.False);
        }

        [Test]
        public void Equals_BoxedValue_ReturnsTrue()
        {
            var traitValue = (TraitValue<int>)9;
            object traitValueB = (TraitValue<int>)9;

            Assert.That(traitValue.Equals(traitValueB), Is.True);
        }

        [Test]
        public void OperatorNotEqual_ValuesAreEqual_ReturnsFalse()
        {
            var traitValueA = (TraitValue<int>)9;
            var traitValueB = (TraitValue<int>)9;

            var result = traitValueA != traitValueB;

            Assert.That(result, Is.False);
        }

        [Test]
        public void OperatorNotEqual_ValuesAreNotEqual_ReturnsTrue()
        {
            var traitValueA = (TraitValue<int>)9;
            var traitValueB = (TraitValue<int>)18;

            var result = traitValueA != traitValueB;

            Assert.That(result, Is.True);
        }

        [Test]
        public void CompareTo_ValueIsEqual_ReturnsZero()
        {
            var traitValueA = (TraitValue<int>)9;
            var traitValueB = (TraitValue<int>)9;

            Assert.That(traitValueA.CompareTo(traitValueB), Is.EqualTo(0));
        }

        [Test]
        public void CompareTo_ValueIsGreater_ReturnsOne()
        {
            var traitValueA = (TraitValue<int>)10;
            var traitValueB = (TraitValue<int>)9;

            Assert.That(traitValueA.CompareTo(traitValueB), Is.EqualTo(1));
        }

        [Test]
        public void CompareTo_ValueIsLess_ReturnsMinusOne()
        {
            var traitValueA = (TraitValue<int>)9;
            var traitValueB = (TraitValue<int>)10;

            Assert.That(traitValueA.CompareTo(traitValueB), Is.EqualTo(-1));
        }
    }
}
