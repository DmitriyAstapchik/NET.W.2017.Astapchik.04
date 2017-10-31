using System;
using System.Text;

namespace Homework
{
    /// <summary>
    /// Class contains extension method that can represent double-precision floating-point number in IEEE 754 format
    /// </summary>
    public static class IEEE754Extension
    {
        /// <summary>
        /// character for displaying bit value 1
        /// </summary>
        private const char ONE = '1';

        /// <summary>
        /// character for displaying bit value 0
        /// </summary>
        private const char ZERO = '0';

        /// <summary>
        /// notation base of number representation
        /// </summary>
        private const byte BASE = 2;

        /// <summary>
        /// length of exponent in IEEE754 double representation
        /// </summary>
        private const byte EXPONENT_LENGTH = 11;

        /// <summary>
        /// length of mantissa in IEEE754 double representation
        /// </summary>
        private const byte MANTISSA_LENGTH = 52;

        /// <summary>
        /// Extension method that represent double-precision number in IEEE 754 format
        /// </summary>
        /// <param name="number">number to represent</param>
        /// <returns>string representation of <paramref name="number"/></returns>
        public static string ToIEEE754String(this double number)
        {
            #region special cases
            if (number == 0d && double.IsNegativeInfinity(1d / number))
            {
                return ONE + new string(ZERO, EXPONENT_LENGTH) + new string(ZERO, MANTISSA_LENGTH);
            }

            if (number == 0d && double.IsPositiveInfinity(1d / number))
            {
                return ZERO + new string(ZERO, EXPONENT_LENGTH) + new string(ZERO, MANTISSA_LENGTH);
            }

            if (number == double.NegativeInfinity)
            {
                return ONE + new string(ONE, EXPONENT_LENGTH) + new string(ZERO, MANTISSA_LENGTH);
            }

            if (number == double.PositiveInfinity)
            {
                return ZERO + new string(ONE, EXPONENT_LENGTH) + new string(ZERO, MANTISSA_LENGTH);
            }

            if (number == double.MinValue)
            {
                return ONE + new string(ONE, EXPONENT_LENGTH - 1) + ZERO + new string(ONE, MANTISSA_LENGTH);
            }

            if (number == double.MaxValue)
            {
                return ZERO + new string(ONE, EXPONENT_LENGTH - 1) + ZERO + new string(ONE, MANTISSA_LENGTH);
            }

            if (number == double.Epsilon)
            {
                return ZERO + new string(ZERO, EXPONENT_LENGTH) + new string(ZERO, MANTISSA_LENGTH - 1) + ONE;
            }

            if (double.IsNaN(number))
            {
                return ONE + new string(ONE, EXPONENT_LENGTH) + ONE + new string(ZERO, MANTISSA_LENGTH - 1);
            }
            #endregion

            return GetSign(ref number) + GetExponent(number) + GetMantissa(number);
        }

        /// <summary>
        /// gets leftmost bit of <paramref name="number"/> as string
        /// </summary>
        /// <param name="number">double-precision number</param>
        /// <returns>0 or 1</returns>
        private static string GetSign(ref double number)
        {
            char sign = number < 0 ? ONE : ZERO;
            number = Math.Abs(number);
            return char.ToString(sign);
        }

        /// <summary>
        /// gets string representation of exponent of <paramref name="number"/> in IEEE754 format
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        private static string GetExponent(double number)
        {
            var binary = new StringBuilder(0, EXPONENT_LENGTH);
            var exp = Math.Floor(Math.Log(number, BASE)) + ((1 << EXPONENT_LENGTH - 1) - 1);

            for (int i = 0, pow = EXPONENT_LENGTH - 1; i < binary.MaxCapacity; i++, pow--)
            {
                if (Math.Pow(BASE, pow) <= exp)
                {
                    binary.Append(ONE);
                    exp -= Math.Pow(BASE, pow);
                }
                else
                {
                    binary.Append(ZERO);
                }
            }

            return binary.ToString();
        }

        /// <summary>
        /// gets mantissa string representation of <paramref name="number"/> in IEEE754 format
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        private static string GetMantissa(double number)
        {
            var binary = new StringBuilder(0, MANTISSA_LENGTH + 1);

            for (int i = 0, pow = (int)Math.Floor(Math.Log(number, BASE)); i < binary.MaxCapacity; i++, pow--)
            {
                if (Math.Pow(BASE, pow) <= number)
                {
                    binary.Append(ONE);
                    number -= Math.Pow(BASE, pow);
                }
                else
                {
                    binary.Append(ZERO);
                }
            }

            return binary.Remove(0, 1).ToString();
        }
    }
}
