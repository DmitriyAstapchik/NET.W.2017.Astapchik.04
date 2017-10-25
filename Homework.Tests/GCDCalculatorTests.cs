using NUnit.Framework;
using System;
using static Homework.GCDCalculator;

namespace Homework.Tests
{
    [TestFixture]
    public class GCDCalculatorTests
    {
        static Random rand = new Random();

        [TestCase(1, 2, 3, 4, 0, 0, 1, -2, -3, -55, -111, 123, ExpectedResult = 1)]
        [TestCase(123, 123, 123, 123, 123, ExpectedResult = 123)]
        [TestCase(-3, -3, -3, -3, ExpectedResult = 3)]
        [TestCase(0, 0, 0, 0, 0, 0, 0, ExpectedResult = 0)]
        [TestCase(0, -10, 0, -11, 0, 512, ExpectedResult = 0)]
        [TestCase(20, 100, 0, 0, -150, -1000, 25, -5, 25, 0, 20, 105, ExpectedResult = 5)]
        [TestCase(9999, -9999, 999, 999, -999, 999999, 9, ExpectedResult = 9)]
        [TestCase(123, 901, 55, 2223, -124, ExpectedResult = 1)]
        [TestCase(-20, -10, -30, -10, -2300, ExpectedResult = 10)]
        [TestCase(385, 0, -1089, -11, 121, 627, 2838, -105457, ExpectedResult = 11)]
        public uint CalculateGCDTest(params int[] numbers)
        {
            var resultEuc = CalculateEuclideanGCD(out long eucTicks, numbers);
            System.Diagnostics.Debug.WriteLine($"euclidean ticks: {eucTicks}");

            var resultBin = CalculateSteinsGCD(out long binTicks, numbers);
            System.Diagnostics.Debug.WriteLine($"binary ticks: {binTicks}");

            Assert.That(resultBin == resultEuc);
            return resultEuc;
        }

        [Test]
        public void CalculateGCDTestAuto()
        {
            const uint TESTS = 50;

            for (uint i = 0; i < TESTS; i++)
            {
                byte gcd = (byte)rand.Next(0, 256);
                Assert.AreEqual(gcd, CalculateGCDTest(GenerateGCDNumbers(gcd)));
            }

            int[] GenerateGCDNumbers(byte gcd)
            {
                const int MIN = 2;
                const int MAX = 200_000;

                var numbers = new int[rand.Next(MIN, MAX)];
                for (int i = 0; i < numbers.Length; i++)
                {
                    numbers[i] = rand.Next(int.MinValue / (gcd + 1), int.MaxValue / (gcd + 1)) * gcd;
                }

                return numbers;
            }
        }
    }

    [TestFixture]
    public class IEEE754ExtensionTests
    {
        [TestCase(-255.255, ExpectedResult = "1100000001101111111010000010100011110101110000101000111101011100")]
        [TestCase(255.255, ExpectedResult = "0100000001101111111010000010100011110101110000101000111101011100")]
        [TestCase(4294967295.0, ExpectedResult = "0100000111101111111111111111111111111111111000000000000000000000")]
        [TestCase(double.MinValue, ExpectedResult = "1111111111101111111111111111111111111111111111111111111111111111")]
        [TestCase(double.MaxValue, ExpectedResult = "0111111111101111111111111111111111111111111111111111111111111111")]
        [TestCase(double.Epsilon, ExpectedResult = "0000000000000000000000000000000000000000000000000000000000000001")]
        [TestCase(double.NaN, ExpectedResult = "1111111111111000000000000000000000000000000000000000000000000000")]
        [TestCase(double.NegativeInfinity, ExpectedResult = "1111111111110000000000000000000000000000000000000000000000000000")]
        [TestCase(double.PositiveInfinity, ExpectedResult = "0111111111110000000000000000000000000000000000000000000000000000")]
        [TestCase(0.0, ExpectedResult = "0000000000000000000000000000000000000000000000000000000000000000")]
        [TestCase(-0.0, ExpectedResult = "1000000000000000000000000000000000000000000000000000000000000000")]
        [TestCase(0.1238279, ExpectedResult = "0011111110111111101100110010111101101100110101010001010101110001")]
        [TestCase(0.8188472919, ExpectedResult = "0011111111101010001100111111111100111100011001000001111000001000")]
        [TestCase(19283791824, ExpectedResult = "0100001000010001111101011001110100111111010000000000000000000000")]
        [TestCase(-838528782, ExpectedResult = "1100000111001000111111010111011110000111000000000000000000000000")]
        public string ToIEEE754StringTest(double number)
        {
            return number.ToIEEE754String();
        }
    }
}
