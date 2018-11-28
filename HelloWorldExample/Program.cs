// <copyright file="Program.cs" company="Henning Moe">
// Copyright (c) Henning Moe. All rights reserved.
// </copyright>

namespace HelloWorldExample
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Genetics;

    /// <summary>
    /// Program entrypoint for Hello World
    /// </summary>
    internal class Program
    {
        private static readonly Random Rnd = new Random();

        // Create a lookup table. The ASCII table (which the lower 7-bit of unicode codes encode) has a fairly arbitrary layout
        // In order to reduce the amount of times the algorithm will hit genetic dead ends we'll just map a smaller set of symbols.
        private static readonly char[] Lookup =
            Enumerable.Range('a', 'z' - 'a')
            .Concat(Enumerable.Range('A', 'Z' - 'A'))
            .Select(x => (char)x)
            .Concat(new char[] { ' ', '!', ',' })
            .OrderBy(a => Rnd.Next()) // Scramble!
            .ToArray();

        private static readonly Memory<byte> HelloString = new Memory<byte>("Hello, World!".ToCharArray().Select(LookupChar).ToArray());
        private static readonly Memory<byte> CheerString = new Memory<byte>("Cheers, Mate!".ToCharArray().Select(LookupChar).ToArray());

#pragma warning disable SA1008 // Opening parenthesis must be spaced correctly. StyleCop doesn't recognize tuples.

        // I desperately wanted a one-liner.
        private static byte LookupChar(char value) => (byte)Lookup.Select((x, i) => (x, i)).Aggregate(255, (a, b) => b.x == value ? b.i : a);

#pragma warning restore SA1008 // Opening parenthesis must be spaced correctly

        private static char LookupIndex(byte index) => Lookup[index % Lookup.Length];

        private static Trait<byte, int> GenerateTrait(int index)
        {
            return new Trait<byte, int>((byte)index, (TraitValue<int>)Rnd.Next(0, Lookup.Length));
        }

        private static Genes<byte, int> NewRandomGenes()
        {
            var values = Enumerable.Range(0, HelloString.Length).Select(GenerateTrait).ToArray();
            return new Genes<byte, int>(values);
        }

        private static void Main(string[] args)
        {
#pragma warning disable SA1008 // Opening parenthesis must be spaced correctly. StyleCop doesn't recognize tuples.
            var combiner = new GeneCombiner<byte, int>(Mutators.Gaussian, (1, 10));
#pragma warning restore SA1008 // Opening parenthesis must be spaced correctly
            var generation = Enumerable.Range(0, 200).Select(j => NewRandomGenes()).ToList();

            int count = 0;
            while (true)
            {
                generation = NextGeneration(generation, combiner);
                ++count;
                if (generation is null)
                {
                    Console.WriteLine("Completed in {0} generations.", count);
                    break;
                }
            }
        }

        private static string Write(Genes<byte, int> g)
        {
            var builder = new StringBuilder(HelloString.Length);
            for (int i = 0; i < g.Count; ++i)
            {
                builder.Append(LookupIndex((byte)g[i].Value));
            }

            var result = builder.ToString();
            Console.WriteLine(result);
            return result;
        }

        /// <summary>
        /// Creates a new generation using the specified combiner
        /// </summary>
        /// <param name="generation">Generation to combine</param>
        /// <param name="combiner">Combiner to use</param>
        /// <returns>A list of genes for the new generation or null if the most fit gene set is a greeting</returns>
        private static List<Genes<byte, int>> NextGeneration(IEnumerable<Genes<byte, int>> generation, GeneCombiner<byte, int> combiner)
        {
            var top = generation.Select(x => new { Genes = x, Fitness = FitnessFunction(x) }).OrderBy(x => x.Fitness).ToArray();
            string s = Write(top[0].Genes);

            if (top[0].Fitness == 0)
            {
                // The most fit gene is a greeting
                return null;
            }

            var results = new List<Genes<byte, int>>(top.Length);

            for (int i = 0; i < 50; ++i)
            {
                for (int j = 0; j < 4; ++j)
                {
                    var left = top[Rnd.Next(50)].Genes;
                    var right = top[Rnd.Next(50)].Genes;

                    results.Add(combiner.Combine(left, right));
                }
            }

            return results;
        }

        /// <summary>
        /// Implements a fitness function for a set of genes
        /// </summary>
        /// <param name="genes">Genes to calculate fitness of</param>
        /// <returns>The fitness of the specified gene</returns>
        private static double FitnessFunction(Genes<byte, int> genes)
        {
            Span<byte> value = stackalloc byte[genes.Count];
            for (int i = 0; i < genes.Count; ++i)
            {
                value[i] = (byte)genes[i].Value;
            }

            return Math.Min(LevenshteinDistance(value, HelloString.Span), LevenshteinDistance(value, CheerString.Span));
        }

        /// <summary>
        /// Implements the Levenshtein distance function.
        /// </summary>
        /// <param name="left">String1</param>
        /// <param name="right">String2</param>
        /// <returns>Edit actions required between the two</returns>
        /// <remarks>This code is taken from Sam Allen's Lehvenstein Distance implementation on <a href="https://www.dotnetperls.com/levenshtein">Dotnet pearls</a></remarks>
        private static int LevenshteinDistance(Span<byte> left, Span<byte> right)
        {
            if (left.Length == 0)
            {
                return right.Length;
            }

            if (right.Length == 0)
            {
                return left.Length;
            }

            var d = new int[left.Length + 1, right.Length + 1];

            for (int i = 1; i <= left.Length; ++i)
            {
                d[i, 0] = i;
            }

            for (int j = 1; j <= right.Length; ++j)
            {
                d[0, j] = j;
            }

            for (int i = 1; i <= left.Length; i++)
            {
                for (int j = 1; j <= right.Length; j++)
                {
                    int cost = (right[j - 1] == left[i - 1]) ? 0 : 1;

                    d[i, j] = Math.Min(
                        Math.Min(d[i - 1, j] + 1, d[i, j - 1] + 1),
                        d[i - 1, j - 1] + cost);
                }
            }

            return d[left.Length, right.Length];
        }
    }
}
