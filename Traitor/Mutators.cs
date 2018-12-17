// <copyright file="Mutators.cs" company="Henning Moe">
// Copyright (c) Henning Moe. All rights reserved.
// </copyright>

namespace Traitor
{
    using System;

    /// <summary>
    /// Defines a delegate type for mutation
    /// </summary>
    /// <typeparam name="T">Type of value to mutate</typeparam>
    /// <param name="input">Previous value</param>
    /// <returns>Mutated value</returns>
    public delegate T Mutator<T>(T input);

    /// <summary>
    /// Defines common mutators
    /// </summary>
    public static class Mutators
    {
        private static readonly Random RandomInstance = new Random();

        /// <summary>
        /// Implements a simple +/- mutator
        /// </summary>
        /// <param name="input">Previous value</param>
        /// <returns>Either returns input + 1 or input - 1</returns>
        public static int Increment(int input) => input + RandomInstance.Next(2) == 0 ? 1 : -1;

        /// <summary>
        /// Implements a mutator with guassian distribution
        /// </summary>
        /// <param name="input">Previous value</param>
        /// <returns>Return input +/- 1, 2 or 3</returns>
        public static int Gaussian(int input) => input + (int)(3 - (Gauss((RandomInstance.NextDouble() * 2) - 1) * 3));

        /// <summary>
        /// Implements a mutator with guassian distribution
        /// </summary>
        /// <param name="input">Previous value</param>
        /// <returns>Returns input +/- [1 3]</returns>
        public static float Gaussian(float input) => input + 1 - (float)Gauss((RandomInstance.NextDouble() * 2) - 1);

        /// <summary>
        /// Implements a mutator with guassian distribution
        /// </summary>
        /// <param name="input">Previous value</param>
        /// <returns>Returns input +/- [1 3]</returns>
        public static double Gaussian(double input) => input + 1 - Gauss((RandomInstance.NextDouble() * 2) - 1);

        /// <summary>
        /// Implements exponential distribution
        /// </summary>
        /// <param name="input">Previous value</param>
        /// <returns>Input +/- an exponentially distributed value</returns>
        public static double Exponential(double input) => input + (input * input * (RandomInstance.Next(2) == 0 ? -1 : 1));

        private static double Gauss(double x, double a = 1.0, double b = 0, double c = 0.2) => Math.Pow(a * Math.E, -(Math.Pow(x - b, 2) / ((2 * c) * (2 * c))));
    }
}
