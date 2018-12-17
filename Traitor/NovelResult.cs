// <copyright file="NovelResult.cs" company="Henning Moe">
// Copyright (c) Henning Moe. All rights reserved.
// </copyright>

namespace Traitor
{
    using System;

    /// <summary>
    /// Provides a result from a novel trait mutation
    /// </summary>
    /// <typeparam name="TKey">Trait key type</typeparam>
    /// <typeparam name="TValue">Trait value type</typeparam>
    public readonly struct NovelResult<TKey, TValue>
        where TKey : IEquatable<TKey>
        where TValue : struct, IEquatable<TValue>, IComparable<TValue>, IFormattable
    {
        /// <summary>
        /// Gets the trait that should be added or removed from the gene set
        /// </summary>
        public readonly Trait<TKey, TValue> Result;

        /// <summary>
        /// Gets a value indicating what to do with the result
        /// </summary>
        public readonly NovelResultType Type;

        /// <summary>
        /// Initializes a new instance of the <see cref="NovelResult{TKey, TValue}"/> struct.
        /// </summary>
        /// <param name="result">Trait to add or remove</param>
        /// <param name="type">Whether the trait should be added or removed</param>
        public NovelResult(Trait<TKey, TValue> result, NovelResultType type)
        {
            this.Result = result;
            this.Type = type;
        }

        /// <summary>
        /// Gets an empty result that does nothing
        /// </summary>
        public static NovelResult<TKey, TValue> None => new NovelResult<TKey, TValue>(default, NovelResultType.None);
    }

    /// <summary>
    /// Convinience functions for creating trait results
    /// </summary>
    public static class NovelResult
    {
        /// <summary>
        /// Creates a NovelResult that adds the novel trait to the gene set
        /// </summary>
        /// <typeparam name="TKey">Trait key type</typeparam>
        /// <typeparam name="TValue">Trait value type</typeparam>
        /// <param name="key">Key to add</param>
        /// <param name="value">Trait value to add</param>
        /// <returns>A novel trait result that should be added to a gene set</returns>
        public static NovelResult<TKey, TValue> Add<TKey, TValue>(TKey key, TValue value)
            where TKey : IEquatable<TKey>
            where TValue : struct, IEquatable<TValue>, IComparable<TValue>, IFormattable
        {
            return new NovelResult<TKey, TValue>(new Trait<TKey, TValue>(key, value), NovelResultType.Add);
        }

        /// <summary>
        /// Creates a NovelResult that removes a trait from a gene set
        /// </summary>
        /// <typeparam name="TKey">Trait key type</typeparam>
        /// <typeparam name="TValue">Trait value type</typeparam>
        /// <param name="key">Key to remove</param>
        /// <param name="value">Trait value. This is provided since C# cannot do partial generic type inference and this value is usually ignored.</param>
        /// <returns>A novel trait result that should be removed from a gene set</returns>
        public static NovelResult<TKey, TValue> Remove<TKey, TValue>(TKey key, TValue value)
            where TKey : IEquatable<TKey>
            where TValue : struct, IEquatable<TValue>, IComparable<TValue>, IFormattable
        {
            return new NovelResult<TKey, TValue>(new Trait<TKey, TValue>(key, value), NovelResultType.Remove);
        }
    }
}
