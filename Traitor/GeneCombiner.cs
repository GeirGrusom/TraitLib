// <copyright file="GeneCombiner.cs" company="Henning Moe">
// Copyright (c) Henning Moe. All rights reserved.
// </copyright>

namespace Traitor
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// This class is used to combine one or more gene sets.
    /// </summary>
    /// <typeparam name="TKey">Gene key type used. This is typically string or int</typeparam>
    /// <typeparam name="TValue">Gene value type. This is typically int or float</typeparam>
    public sealed class GeneCombiner<TKey, TValue>
        where TKey : IEquatable<TKey>
        where TValue : struct, IEquatable<TValue>, IComparable<TValue>, IFormattable
    {
        [ThreadStatic]
        private static Random randomSource;

        private readonly Func<int> rand;
        private readonly Func<IEnumerable<TKey>, NovelResult<TKey, TValue>> traitFactory;
        private readonly Mutator<TValue> mutationValue;
        private readonly int mutationChance;
        private readonly int novelTraitChance;

        /// <summary>
        /// Initializes a new instance of the <see cref="GeneCombiner{TKey, TValue}"/> class.
        /// </summary>
        /// <param name="random">Random function used by gene combination</param>
        /// <param name="traitFactory">Factory used to create novel traits</param>
        /// <param name="mutationValue">Function used to mutate a trait value with common functions defined in <see cref="Mutators"/></param>
        /// <param name="mutationChance">Mutation chance as a rational number</param>
        /// <param name="novelTraitChance">Novel trait chance as a rational number</param>
        /// <exception cref="OverflowException">Thrown if either mutation chance or novel trait chance ends up being less than int.MaxValue</exception>
        public GeneCombiner(Func<int> random, Func<IEnumerable<TKey>, NovelResult<TKey, TValue>> traitFactory, Mutator<TValue> mutationValue, Rational mutationChance, Rational novelTraitChance)
        {
            this.rand = random;
            this.traitFactory = traitFactory;
            this.mutationValue = mutationValue;
            this.mutationChance = checked(mutationChance.Numerator * mutationChance.Denominator);
            this.novelTraitChance = checked(novelTraitChance.Numerator * novelTraitChance.Denominator);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GeneCombiner{TKey, TValue}"/> class.
        /// </summary>
        /// <param name="traitFactory">Factory used to create novel traits</param>
        /// <param name="mutationValue">Function used to mutate a trait value with common functions defined in <see cref="Mutators"/></param>
        /// <param name="mutationChance">Mutation chance as a rational number</param>
        /// <param name="novelTraitChance">Novel trait chance as a rational number</param>
        public GeneCombiner(Func<IEnumerable<TKey>, NovelResult<TKey, TValue>> traitFactory, Mutator<TValue> mutationValue, Rational mutationChance, Rational novelTraitChance)
            : this(NextRandom, traitFactory, mutationValue, mutationChance, novelTraitChance)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GeneCombiner{TKey, TValue}"/> class.
        /// This constructor does not produce novel traits.
        /// </summary>
        /// <param name="mutationValue">Function used to mutate a trait value with common functions defined in <see cref="Mutators"/></param>
        /// <param name="mutationChance">Mutation chance as a rational number</param>
        public GeneCombiner(Mutator<TValue> mutationValue, Rational mutationChance)
            : this(inp => throw new NotSupportedException(), mutationValue, mutationChance, Rational.Zero)
        {
        }

        /// <summary>
        /// Creates a random set of genes using the provided novel trait factory
        /// </summary>
        /// <param name="minTraits">Specifies the minimum number of traits in each new gene set</param>
        /// <param name="maxTraits">Specifies the maximum number of traits in each new gene set</param>
        /// <returns>An unending set of genes</returns>
        public IEnumerable<Genes<TKey, TValue>> CreateRandomSet(int minTraits, int maxTraits)
        {
            while (true)
            {
                var newTraits = new List<Trait<TKey, TValue>>();
                var traits = NextRandom(minTraits, maxTraits + 1);
                for (int i = 0; i < maxTraits - (maxTraits - minTraits); ++i)
                {
                    NovelResult<TKey, TValue> newTrait;
                    int tries = 0;
                    do
                    {
                        newTrait = this.traitFactory(newTraits.Select(x => x.Key));

                        if (++tries == 10)
                        {
                            goto DontAdd;
                        }
                    }
                    while (newTrait.Type != NovelResultType.Add);

                    newTraits.Add(newTrait.Result);

                    DontAdd:
                    {
                    }
                }

                yield return new Genes<TKey, TValue>(newTraits);
            }
        }

        /// <summary>
        /// Creates a new set of genes with random selection from either parent.
        /// If a specific trait is not present in one of the parents it has a 50% chance of not being expressed at all.
        /// </summary>
        /// <param name="initial">Initial geneset</param>
        /// <param name="genes">Genes to combine. This can be empty, for asexual reproduction, but it cannot be null.</param>
        /// <returns>Combined set of traits from each parent.</returns>
        /// <exception cref="ArgumentNullException">Thrown when either initial or genes is null.</exception>
        public Genes<TKey, TValue> Combine(Genes<TKey, TValue> initial, params Genes<TKey, TValue>[] genes)
        {
            if (initial is null)
            {
                throw new ArgumentNullException(nameof(initial));
            }

            if (genes is null)
            {
                throw new ArgumentNullException(nameof(genes));
            }

            var results = new List<Trait<TKey, TValue>>(Max(initial.Count, genes));

            var allKeys = initial.GetKeys().Union(genes.SelectMany(x => x.GetKeys()).Distinct()).ToArray();

            for (int i = 0; i < allKeys.Length; ++i)
            {
                var key = allKeys[i];

                int index = this.rand() % (genes.Length + 1);
                var source = index == 0 ? initial : genes[index - 1];

                if (!source.TryGet(key, out var value))
                {
                    // The selected parent didn't express the gene.
                    continue;
                }

                if (this.mutationChance > 0 && this.rand() % this.mutationChance == 0)
                {
                    value = (TraitValue<TValue>)this.mutationValue((TValue)value);
                }

                results.Add(new Trait<TKey, TValue>(key, value));
            }

            if (this.novelTraitChance > 0 && this.rand() % this.novelTraitChance == 0)
            {
                var newTrait = this.traitFactory(results.Select(x => x.Key));
                if (newTrait.Type == NovelResultType.Add && !results.Any(x => x.Key.Equals(newTrait.Result.Key)))
                {
                    results.Add(newTrait.Result);
                }
                else if (newTrait.Type == NovelResultType.Remove)
                {
                    results.RemoveAll(x => x.Key.Equals(newTrait.Result.Key));
                }
            }

            return new Genes<TKey, TValue>(results);
        }

        private static int NextRandom()
        {
            if (randomSource is null)
            {
                randomSource = new Random();
            }

            return randomSource.Next();
        }

        private static int NextRandom(int min, int max)
        {
            if (randomSource is null)
            {
                randomSource = new Random();
            }

            return randomSource.Next(min, max);
        }

        private static int Max(int initial, Genes<TKey, TValue>[] counts)
        {
            int max = initial;
            foreach (var geneSet in counts)
            {
                if (max < geneSet.Count)
                {
                    max = geneSet.Count;
                }
            }

            return max;
        }
    }
}
