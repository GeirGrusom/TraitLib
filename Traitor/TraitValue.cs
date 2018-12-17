// <copyright file="TraitValue.cs" company="Henning Moe">
// Copyright (c) Henning Moe. All rights reserved.
// </copyright>

namespace Traitor
{
    using System;

    /// <summary>
    /// Implements a strongly typed trait value as a value type
    /// </summary>
    /// <typeparam name="TValue">Inner value type of this trait. Commonly either int or float</typeparam>
    public readonly struct TraitValue<TValue> : IEquatable<TraitValue<TValue>>, IComparable<TraitValue<TValue>>, IFormattable
        where TValue : struct, IEquatable<TValue>, IComparable<TValue>, IFormattable
    {
        private readonly TValue expression;

        private TraitValue(TValue value)
        {
            this.expression = value;
        }

        /// <summary>
        /// Implements an explicit cast to the underlying type
        /// </summary>
        /// <param name="trait">Trait to cast</param>
        public static explicit operator TValue(TraitValue<TValue> trait) => trait.expression;

        /// <summary>
        /// Implements an explicit cast to trait from the underlying type
        /// </summary>
        /// <param name="trait">Trait to cast</param>
        public static explicit operator TraitValue<TValue>(TValue trait) => new TraitValue<TValue>(trait);

        /// <summary>
        /// Implements equals operator as a value comparer
        /// </summary>
        /// <param name="lhs">Left hand operand</param>
        /// <param name="rhs">Right hand operand</param>
        /// <returns>True if the values are equal, otherwise false</returns>
        public static bool operator ==(TraitValue<TValue> lhs, TraitValue<TValue> rhs) => lhs.Equals(rhs);

        /// <summary>
        /// Implements unequal operator as a value comparer
        /// </summary>
        /// <param name="lhs">Left hand operand</param>
        /// <param name="rhs">Right hand operand</param>
        /// <returns>True if the values are not equal, otherwise false</returns>
        public static bool operator !=(TraitValue<TValue> lhs, TraitValue<TValue> rhs) => !lhs.Equals(rhs);

        /// <inheritdoc/>
        public override int GetHashCode()
        {
            return this.expression.GetHashCode();
        }

        /// <inheritdoc/>
        public override string ToString()
        {
            return this.expression.ToString();
        }

        /// <inheritdoc/>
        public int CompareTo(TraitValue<TValue> other)
        {
            return this.expression.CompareTo(other.expression);
        }

        /// <inheritdoc/>
        public string ToString(string format, IFormatProvider formatProvider)
        {
            return this.expression.ToString(format, formatProvider);
        }

        /// <inheritdoc/>
        public bool Equals(TraitValue<TValue> other)
        {
            return this.expression.Equals(other.expression);
        }

        /// <inheritdoc/>
        public override bool Equals(object obj)
        {
            if (obj is null)
            {
                return false;
            }

            if (obj is TraitValue<TValue> tv)
            {
                return this.Equals(tv);
            }

            return false;
        }
    }
}
