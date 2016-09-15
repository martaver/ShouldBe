using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShouldBeAssertions.Tools
{
	public static class Randoms
	{
		private static readonly Random R = new Random();

		public static double NextDouble()
		{
			return R.NextDouble();
		}

		public static int NextInt(int min = Int32.MinValue, int max = Int32.MaxValue)
		{
			if (min == max) return min;
			return R.Next(min, max);
		}

		public static double NextDouble(double min, double max)
		{
			if (min == max) return min;
			return min + (R.NextDouble() * (max - min));
		}

		public static decimal NextDecimal(decimal min = Decimal.MinValue, decimal max = Decimal.MaxValue)
		{
			if (min == max) return min;
			return min + ((decimal)R.NextDouble() * (max - min));
		}

		public static DateTime NextDateTime(DateTime min, DateTime max)
		{
			if (min == max) return min;
			var period = max - min;
			var random = R.Next(0, (int)period.TotalSeconds);
			return min + TimeSpan.FromSeconds(random);
		}

		/// <summary>
		/// Returns a randomly generated string, containing only alpha chars a-z, optionally including capitals.
		/// </summary>
		/// <param name="length">The length of the random string to generate.</param>
		/// <returns>A string of length 'length', containing only random alpha characters.</returns>
		public static string CreateRandomAlpha(int length, bool includeUpper = false)
		{
			if (length <= 0) throw new ArgumentException("must be greater than zero", "length");

			var builder = new StringBuilder(length);

			for (var i = 1; i <= length; i++)
			{
				var randomChar = includeUpper
					? Chars.AlphaLowerUpper[R.Next(Chars.AlphaLowerUpper.Length)]
					: Chars.AlphaLower[R.Next(Chars.AlphaLower.Length)];
				builder.Append(randomChar);
			}

			return builder.ToString();
		}

		/// <summary>
		/// Returns a randomly generated string, containing only alphanumeric chars a-z, 0-9, optionally including capitals.
		/// </summary>
		/// <param name="length">The length of the random string to generate.</param>
		/// <returns>A string of length 'length', containing only random alphanumeric characters.</returns>
		public static string CreateRandomAlphaNumeric(int length, bool includeUpper = false)
		{
			if (length <= 0) throw new ArgumentException("must be greater than zero", "length");

			var builder = new StringBuilder(length);

			for (var i = 1; i <= length; i++)
			{
				var randomChar = includeUpper
					? Chars.AlphaNumericLowerUpper[R.Next(Chars.AlphaNumericLowerUpper.Length)]
					: Chars.AlphaNumericLower[R.Next(Chars.AlphaNumericLower.Length)];
				builder.Append(randomChar);
			}

			return builder.ToString();
		}

		public static T Choose<T>(IEnumerable<T> values)
		{
			var materialised = values.ToArray();
			var max = materialised.Length;
			return materialised[NextInt(0, max - 1)];
		}

		public static T Random<T>(this IEnumerable<T> values)
		{
			return Choose(values);
		}
	}

	public static class Chars
	{
		public static readonly char[] Digits;
		public static readonly char[] AlphaLower;
		public static readonly char[] AlphaUpper;
		public static readonly char[] AlphaLowerUpper;
		public static readonly char[] AlphaNumericLower;
		public static readonly char[] AlphaNumericLowerUpper;

		static Chars()
		{
			//Basic sets
			Digits = Enumerable.Range(48, 9).Select(Convert.ToChar).ToArray(); //chars 48-57 (digits 0-9)
			AlphaLower = Enumerable.Range(97, 25).Select(Convert.ToChar).ToArray(); //chars 97-122 (alpha a-z)
			AlphaUpper = Enumerable.Range(65, 25).Select(Convert.ToChar).ToArray(); //chars 65-90 (alpha A-Z)

			//Combined sets
			AlphaLowerUpper = AlphaLower.Union(AlphaUpper).ToArray();
			AlphaNumericLower = Digits.Union(AlphaLower).ToArray();
			AlphaNumericLowerUpper = Digits.Union(AlphaLowerUpper).ToArray();
		}
	}


}
