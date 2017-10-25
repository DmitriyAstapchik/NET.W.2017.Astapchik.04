using System;
using System.Text;

namespace Homework
{
    /// <summary>
    /// Class contains extension method that can represent double-precision floating-point number in IEEE 754 format
    /// </summary>
    public static class IEEE754Extension
    {
        const char ONE = '1';
        const char ZERO = '0';
        const byte BASE = 2;
        const byte EXPONENT_LENGTH = 11;
        const byte MANTISSA_LENGTH = 52;

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

        static string GetSign(ref double number)
        {
            char sign = number < 0 ? ONE : ZERO;
            number = Math.Abs(number);
            return char.ToString(sign);
        }

        static string GetExponent(double number)
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

        static string GetMantissa(double number)
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
