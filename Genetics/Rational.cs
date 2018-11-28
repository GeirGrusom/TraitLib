// <copyright file="Rational.cs" company="Henning Moe">
// Copyright (c) Henning Moe. All rights reserved.
// </copyright>

namespace Genetics
{
    /// <summary>
    /// Implements a simple rational structure
    /// </summary>
    public readonly struct Rational
    {
        /// <summary>
        /// Gets an identity, (1 / 1),  rational
        /// </summary>
        public static readonly Rational Identity = new Rational(1, 1);

        /// <summary>
        /// Gets a zero rational which does not cause a division by zero (0 / 1)
        /// </summary>
        public static readonly Rational Zero = new Rational(0, 1);

        /// <summary>
        /// Gets the numerator used by this rational
        /// </summary>
        public readonly int Numerator;

        /// <summary>
        /// Gets the denominator used by this rational
        /// </summary>
        public readonly int Denominator;

        /// <summary>
        /// Initializes a new instance of the <see cref="Rational"/> struct.
        /// </summary>
        /// <param name="num">Numerator to use</param>
        /// <param name="den">Denominator to use</param>
        public Rational(int num, int den)
        {
            this.Numerator = num;
            this.Denominator = den;
        }

        /// <summary>
        /// Implements a cast from a tuple to a rational
        /// </summary>
        /// <param name="src">Tuple to case</param>
        public static implicit operator Rational((int numerator, int denominator) src)
        {
            return new Rational(src.numerator, src.denominator);
        }

        /// <inheritdoc/>
        public override string ToString()
        {
            return $"{this.Numerator} / {this.Denominator}";
        }
    }
}
