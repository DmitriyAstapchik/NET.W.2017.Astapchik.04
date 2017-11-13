using System;
using System.Diagnostics;

namespace Homework
{
    /// <summary>
    /// Class can calculate GCD of any set of numbers using two different algorithms
    /// </summary>
    public static class GCDCalculator
    {
        /// <summary>
        /// Calculates GCD of any set of integers using Euclidean algorithm
        /// </summary>
        /// <param name="numbers">input integers</param>
        /// <returns>GCD</returns>
        public static uint CalculateEuclideanGCD(params int[] numbers)
        {
            return CalculateGCD(out _, EuclideanAlgorithm, numbers);
        }

        /// <summary>
        /// Calculates GCD of any set of integers using Euclidean algorithm and counts algorithm ticks
        /// </summary>
        /// <param name="ticks">elapsed ticks</param>
        /// <param name="numbers">input numbers</param>
        /// <returns>GCD of <paramref name="numbers"/></returns>
        public static uint CalculateEuclideanGCD(out long ticks, params int[] numbers)
        {
            return CalculateGCD(out ticks, EuclideanAlgorithm, numbers);
        }

        /// <summary>
        /// Calculates GCD of any set of integers using Stein's algorithm
        /// </summary>
        /// <param name="numbers">input integers</param>
        /// <returns>GCD</returns>
        public static uint CalculateSteinsGCD(params int[] numbers)
        {
            return CalculateGCD(out _, SteinsAlgorithm, numbers);
        }

        /// <summary>
        /// Calculates GCD of any set of numbers using Stein's algorithm and counts algorithm ticks
        /// </summary>
        /// <param name="ticks">elapsed ticks</param>
        /// <param name="numbers">input numbers</param>
        /// <returns>GCD of <paramref name="numbers"/></returns>
        public static uint CalculateSteinsGCD(out long ticks, params int[] numbers)
        {
            return CalculateGCD(out ticks, SteinsAlgorithm, numbers);
        }

        /// <summary>
        /// Calculates GCD and counts ticks using <paramref name="algorithm"/>
        /// </summary>
        /// <param name="ticks">elapsed ticks</param>
        /// <param name="algorithm">GCD algorithm method</param>
        /// <param name="numbers">input integers</param>
        /// <returns>GCD by <paramref name="algorithm"/></returns>
        /// <exception cref="ArgumentException">numbers array length is less than two</exception>
        private static uint CalculateGCD(out long ticks, Func<uint, uint, uint> algorithm, params int[] numbers)
        {
            if (numbers.Length < 2)
            {
                throw new ArgumentException("Method requires at least 2 numbers.");
            }

            var sw = Stopwatch.StartNew();
            uint gcd = (uint)Math.Abs(numbers[0]);
            for (int i = 1; i < numbers.Length && gcd > 1; i++)
            {
                gcd = algorithm.Invoke(gcd, (uint)Math.Abs(numbers[i]));
            }

            sw.Stop();
            ticks = sw.ElapsedTicks;

            return gcd;
        }

        /// <summary>
        /// Euclidean algorithm for calculating GCD of two non-negative integers
        /// </summary>
        /// <param name="a">first integer</param>
        /// <param name="b">second integer</param>
        /// <returns>GCD of two integers</returns>
        private static uint EuclideanAlgorithm(uint a, uint b)
        {
            if (b == 0)
            {
                return a;
            }

            return EuclideanAlgorithm(b, a % b);
        }

        /// <summary>
        /// Stein's algorithm for calculating GCD of two non-negative integers
        /// </summary>
        /// <param name="a">first integer</param>
        /// <param name="b">second integer</param>
        /// <returns>GCD of two integers</returns>
        private static uint SteinsAlgorithm(uint a, uint b)
        {
            if (a == b)
            {
                return a;
            }

            if (a == 0)
            {
                return b;
            }

            if (b == 0)
            {
                return a;
            }

            if ((~a & 1) != 0)
            {
                if ((b & 1) != 0)
                {
                    return SteinsAlgorithm(a >> 1, b);
                }
                else
                {
                    return SteinsAlgorithm(a >> 1, b >> 1) << 1;
                }
            }

            if ((~b & 1) != 0)
            {
                return SteinsAlgorithm(a, b >> 1);
            }

            if (a > b)
            {
                return SteinsAlgorithm((a - b) >> 1, b);
            }

            return SteinsAlgorithm((b - a) >> 1, a);
        }
    }
}
