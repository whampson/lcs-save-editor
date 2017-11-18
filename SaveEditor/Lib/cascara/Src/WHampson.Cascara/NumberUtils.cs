#region License
/* Copyright (c) 2017 Wes Hampson
 *
 * Permission is hereby granted, free of charge, to any person obtaining a copy
 * of this software and associated documentation files (the "Software"), to deal
 * in the Software without restriction, including without limitation the rights
 * to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the Software is
 * furnished to do so, subject to the following conditions:
 *
 * The above copyright notice and this permission notice shall be included in all
 * copies or substantial portions of the Software.
 *
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
 * SOFTWARE.
 */
#endregion

using System;
using System.Data;
using System.Text.RegularExpressions;

namespace WHampson.Cascara
{
    /// <summary>
    /// Convenience class for various general-purpose tasks related to numbers.
    /// </summary>
    internal static class NumberUtils
    {
        private const string MathExprPattern = @"^[-+*/().\d ]+$";
        private static readonly Regex MathExprRegex = new Regex(MathExprPattern);

        /// <summary>
        /// Computes the result of a mathematical expression string.
        /// </summary>
        /// <param name="expr">
        /// The expression to evaluate.
        /// </param>
        /// <returns>
        /// The value of the expression as a <see cref="double"/>, if the
        /// expression is syntactically valid.
        /// </returns>
        /// <exception cref="FormatException">
        /// Thrown if the expression is not syntactically correct.
        /// </exception>
        /// <exception cref="ArithmeticException">
        /// Caused if the expression evaluates to infinity.
        /// </exception>
        public static double EvaluateExpression(string expr)
        {
            if (!MathExprRegex.IsMatch(expr))
            {
                string msg = string.Format("Invalid math expression '{0}'", expr);
                throw new FormatException(msg);
            }

            object valObj = new DataTable().Compute(expr, null);
            double val = Convert.ToDouble(valObj);
            if (double.IsInfinity(val))
            {
                string msg = string.Format("Expression '{0}' evaluates to infinity.", expr);
                throw new ArithmeticException(msg);
            }

            return val;
        }

        /// <summary>
        /// Checks whether a <see cref="double"/> value is exactly an integer.
        /// </summary>
        /// <param name="d">
        /// The <see cref="double"/> to be checked.
        /// </param>
        /// <returns>
        /// A value indicating whether the given <see cref="double"/> value
        /// is an integer.
        /// </returns>
        public static bool IsInteger(double d)
        {
            return Math.Abs(d % 1) <= (Double.Epsilon * 100);
        }
    }
}
