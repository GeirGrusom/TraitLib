// <copyright file="Trait.cs" company="Henning Moe">
// Copyright (c) Henning Moe. All rights reserved.
// </copyright>

namespace Genetics
{
    using System;

    /// <summary>
    /// Defines a value type for a single trait key/value pair
    /// </summary>
    /// <typeparam name="TKey">Type of the trait key. Typically int or string</typeparam>
    /// <typeparam name="TValue">Type of the trait value. Typically int or float</typeparam>
    public readonly struct Trait<TKey, TValue>
        where TKey : IEquatable<TKey>
        where TValue : struct, IEquatable<TValue>, IComparable<TValue>, IFormattable
    {
        /// <summary>
        /// Gets the key associated with this trait
        /// </summary>
        public readonly TKey Key;

        /// <summary>
        /// Gets the value associated with this trait
        /// </summary>
        public readonly TraitValue<TValue> Value;

        /// <summary>
        /// Initializes a new instance of the <see cref="Trait{TKey, TValue}"/> struct.
        /// </summary>
        /// <param name="key">Key to use for this trait</param>
        /// <param name="value">Value to use for this trait</param>
        public Trait(TKey key, TraitValue<TValue> value)
        {
            this.Key = key;
            this.Value = value;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Trait{TKey, TValue}"/> struct.
        /// </summary>
        /// <param name="key">Key to use for this trait</param>
        /// <param name="value">Value to use for this trait</param>
        public Trait(TKey key, TValue value)
        {
            this.Key = key;
            this.Value = (TraitValue<TValue>)value;
        }
    }
}
