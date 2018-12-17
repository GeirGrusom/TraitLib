// <copyright file="TraitKeyComparer.cs" company="Henning Moe">
// Copyright (c) Henning Moe. All rights reserved.
// </copyright>

namespace Traitor
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Implementes a comparer that compares two traits for their keys
    /// </summary>
    /// <typeparam name="TKey">Key to use for the comparer</typeparam>
    /// <typeparam name="TValue">Value to use for the comparer</typeparam>
    public sealed class TraitKeyComparer<TKey, TValue> : IEqualityComparer<Trait<TKey, TValue>>
        where TKey : IEquatable<TKey>
        where TValue : struct, IEquatable<TValue>, IComparable<TValue>, IFormattable
    {
        /// <summary>
        /// Default instance to use
        /// </summary>
        public static readonly TraitKeyComparer<TKey, TValue> Comparer = new TraitKeyComparer<TKey, TValue>();

        /// <inheritdoc/>
        public bool Equals(Trait<TKey, TValue> x, Trait<TKey, TValue> y)
        {
            return x.Key.Equals(y.Key);
        }

        /// <inheritdoc/>
        public int GetHashCode(Trait<TKey, TValue> obj)
        {
            return obj.Key.GetHashCode();
        }
    }
}
