namespace Genetics.Tests
{
    using System;
    using System.Collections.Generic;
    using NUnit.Framework;

    [TestFixture(TestOf = typeof(GeneCombiner<,>))]
    public class GeneCombinerTests
    {
        private static readonly Func<IEnumerable<int>, Trait<int, int>> Throw = inp => throw new InvalidOperationException();

        [Test]
        public void Combine_LeftValue_ReturnsLeftParentTrait()
        {
            var combiner = new GeneCombiner<int, int>(() => 0, Throw, Mutators.Increment, Rational.Zero, Rational.Zero);

            var leftParent = new Genes<int, int>(new[] { new Trait<int, int>(0, (TraitValue<int>)0) });
            var rightParent = new Genes<int, int>(new[] { new Trait<int, int>(1, (TraitValue<int>)1) });

            var results = combiner.Combine(leftParent, rightParent);

            Assert.That(results.ContainsKey(0), Is.True);
            Assert.That(results.ContainsKey(1), Is.False);
        }

        [Test]
        public void Combine_RightValue_ReturnsLeftParentTrait()
        {
            var combiner = new GeneCombiner<int, int>(() => 1, Throw, Mutators.Increment, Rational.Zero, Rational.Zero);

            var leftParent = new Genes<int, int>(new[] { new Trait<int, int>(0, (TraitValue<int>)0) });
            var rightParent = new Genes<int, int>(new[] { new Trait<int, int>(1, (TraitValue<int>)1) });

            var results = combiner.Combine(leftParent, rightParent);

            Assert.That(results.ContainsKey(0), Is.False);
            Assert.That(results.ContainsKey(1), Is.True);
        }

        [Test]
        public void Combine_NovelTrait_ReturnsNovelTrait()
        {
            var combiner = new GeneCombiner<int, int>(() => 1, inp => new Trait<int, int>(2, (TraitValue<int>)0), Mutators.Increment, Rational.Zero, Rational.Identity);

            var leftParent = new Genes<int, int>(new[] { new Trait<int, int>(0, (TraitValue<int>)0) });
            var rightParent = new Genes<int, int>(new[] { new Trait<int, int>(1, (TraitValue<int>)1) });

            var results = combiner.Combine(leftParent, rightParent);

            Assert.That(results.ContainsKey(2), Is.True);
        }

        [Test]
        public void Combine_Mutation_IncrementsTrait()
        {
            var combiner = new GeneCombiner<int, int>(() => 1, Throw, i => i + 1, Rational.Identity, Rational.Zero);

            var leftParent = Genes<int, int>.Empty;
            var rightParent = new Genes<int, int>(new[] { new Trait<int, int>(1, (TraitValue<int>)1) });

            var results = combiner.Combine(leftParent, rightParent);
            ref var ret = ref results[0];

            Assert.That((int)ret.Value, Is.EqualTo(2));
        }

        [Test]
        public void Combine_Mutation_DecrementsTrait()
        {
            var combiner = new GeneCombiner<int, int>(() => 0, Throw, i => i - 1, Rational.Identity, Rational.Zero);

            var leftParent = new Genes<int, int>(new[] { new Trait<int, int>(1, (TraitValue<int>)1) }); 
            var rightParent = Genes<int, int>.Empty;

            var results = combiner.Combine(leftParent, rightParent);
            ref var ret = ref results[0];

            Assert.That((int)ret.Value, Is.EqualTo(0));
        }
    }
}
