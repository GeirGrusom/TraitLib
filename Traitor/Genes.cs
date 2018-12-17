// <copyright file="Genes.cs" company="Henning Moe">
// Copyright (c) Henning Moe. All rights reserved.
// </copyright>

namespace Traitor
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Defines a gene set
    /// </summary>
    /// <typeparam name="TKey">Identifier for each gene. Usually a string or an int</typeparam>
    /// <typeparam name="TValue">Value type for each gene. Typically int or float.</typeparam>
    public sealed class Genes<TKey, TValue> : IEquatable<Genes<TKey, TValue>>, IEnumerable<Trait<TKey, TValue>>
        where TKey : IEquatable<TKey>
        where TValue : struct, IEquatable<TValue>, IComparable<TValue>, IFormattable
    {
        private static readonly Dictionary<TKey, TraitValue<TValue>> EmptyDictionary = new Dictionary<TKey, TraitValue<TValue>>();

        private readonly Dictionary<TKey, TraitValue<TValue>> traitSet;
        private readonly Trait<TKey, TValue>[] traits;

        /// <summary>
        /// Initializes a new instance of the <see cref="Genes{TKey, TValue}"/> class.
        /// </summary>
        /// <param name="traits">Traits to use for this set</param>
        /// <exception cref="ArgumentNullException">Thrown if traits is null</exception>
        public Genes(IEnumerable<Trait<TKey, TValue>> traits)
        {
            if (traits is null)
            {
                throw new ArgumentNullException(nameof(traits));
            }

            this.traits = traits.ToArray();
            this.traitSet = this.traits.ToDictionary(x => x.Key, x => x.Value);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Genes{TKey, TValue}"/> class.
        /// This initializes an empty geneset.
        /// </summary>
        private Genes()
        {
            this.traits = Array.Empty<Trait<TKey, TValue>>();
            this.traitSet = EmptyDictionary;
        }

        /// <summary>
        /// Gets an empty gene set
        /// </summary>
        public static Genes<TKey, TValue> Empty { get; } = new Genes<TKey, TValue>();

        /// <summary>
        /// Gets the number of traits in this set
        /// </summary>
        public int Count => this.traits.Length;

        /// <summary>
        /// Gets the specified trait at the specified index
        /// </summary>
        /// <param name="index">Index to get the key for</param>
        /// <returns>A reference to the specified trait</returns>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if index is less than zero and index is greater than or equal to the number of traits</exception>
        public ref Trait<TKey, TValue> this[int index]
        {
            get
            {
                if (index < 0 || index >= this.traits.Length)
                {
                    throw new ArgumentOutOfRangeException(nameof(index), index, "Index must be less than " + this.Count + " and greater or equal to zero");
                }

                return ref this.traits[index];
            }
        }

        /// <summary>
        /// Gets a trati value for the specified key
        /// </summary>
        /// <param name="key">Key to get</param>
        /// <returns>Value for the specified trait key</returns>
        /// <exception cref="KeyNotFoundException">The key does not exist in the collection</exception>
        public TraitValue<TValue> this[TKey key] => this.traitSet.TryGetValue(key, out var result) ? result : throw new KeyNotFoundException();

        /// <summary>
        /// Gets all the trait keys in this set
        /// </summary>
        /// <returns>This sets keys as an array</returns>
        public TKey[] GetKeys() => this.traitSet.Keys.ToArray();

        /// <summary>
        /// Gets a value indicating whether the trait with the specified key exists in the set
        /// </summary>
        /// <param name="key">Key to get</param>
        /// <returns>True if the trait with the specified key exists otherwise false</returns>
        public bool ContainsKey(TKey key) => this.traitSet.ContainsKey(key);

        /// <summary>
        /// Tries to get a value for the specified key
        /// </summary>
        /// <param name="key">Key to retrieve a value for</param>
        /// <param name="value">Value to retrieve</param>
        /// <returns>True if the trait is expressed in the gene set otherwise false</returns>
        public bool TryGet(TKey key, out TraitValue<TValue> value) => this.traitSet.TryGetValue(key, out value);

        /// <inheritdoc />
        public bool Equals(Genes<TKey, TValue> other)
        {
            if (other is null || this.Count != other.Count)
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            for (int i = 0; i < this.Count; ++i)
            {
                ref Trait<TKey, TValue> thisTrait = ref this[i];

                if (!other.TryGet(thisTrait.Key, out var otherValue))
                {
                    return false;
                }

                if (thisTrait.Value != otherValue)
                {
                    return false;
                }
            }

            return true;
        }

        /// <inheritdoc/>
        public IEnumerator<Trait<TKey, TValue>> GetEnumerator()
        {
            return ((IEnumerable<Trait<TKey, TValue>>)this.traits).GetEnumerator();
        }

        /// <inheritdoc/>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }
}
