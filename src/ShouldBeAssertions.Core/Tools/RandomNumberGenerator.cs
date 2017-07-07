using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace ShouldBeAssertions.Core.Tools
{
	/// <summary>
	/// Creates a sequence of random, unique, numbers starting at 1.
	/// </summary>
	/// <remarks>
	/// Based on Ploeh's AutoFixture: https://github.com/AutoFixture/AutoFixture/blob/master/Src/AutoFixture/RandomNumericSequenceGenerator.cs
	/// </remarks>
	public class RandomNumberGenerator
	{
		private readonly long[] limits;
		private readonly object syncRoot;
		private readonly Random random;
		private readonly HashSet<long> numbers;
		private long lower;
		private long upper;
		private long count;

		/// <summary>
		/// Initializes a new instance of the <see cref="RandomNumberGenerator" /> class
		/// with the default limits, 255, 32767, and 2147483647.
		/// </summary>
		public RandomNumberGenerator()
			: this(1, Byte.MaxValue, Int16.MaxValue, Int32.MaxValue)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="RandomNumberGenerator" /> class.
		/// </summary>
		/// <param name="limits">A sequence of at least two ascending numbers.</param>
		public RandomNumberGenerator(IEnumerable<long> limits) : this(limits.ToArray()) { }

		/// <summary>
		/// Initializes a new instance of the <see cref="RandomNumberGenerator" /> class.
		/// </summary>
		/// <param name="limits">An array of at least two ascending numbers.</param>
		/// <exception cref="System.ArgumentNullException"></exception>
		/// <exception cref="System.ArgumentException"></exception>
		public RandomNumberGenerator(params long[] limits)
		{
			if (limits == null)
			{
				throw new ArgumentNullException(nameof(limits));
			}

			if (limits.Length < 2)
			{
				throw new ArgumentException("Limits must be at least two ascending numbers.", nameof(limits));
			}

			ValidateThatLimitsAreStrictlyAscending(limits);

			this.limits = limits;
			this.syncRoot = new object();
			this.random = new Random();
			this.numbers = new HashSet<long>();
			this.CreateRange();
		}

		/// <summary>
		/// Gets the sequence of limits.
		/// </summary>
		/// <value>
		/// The sequence of limits.
		/// </value>
		public IEnumerable<long> Limits => this.limits;

		private static void ValidateThatLimitsAreStrictlyAscending(long[] limits)
		{
			if (limits.Zip(limits.Skip(1), (a, b) => a >= b).Any(b => b))
			{
				throw new ArgumentOutOfRangeException(nameof(limits), "Limits must be ascending numbers.");
			}
		}		

		public object Create(Type request)
		{
			if (request == typeof(byte))
			{
				return (byte) this.GetNextRandom();
			}
			if (request == typeof(decimal))
			{
				return (decimal) this.GetNextRandom();
			}
			if (request == typeof(double))
			{
				return (double) this.GetNextRandom();
			}
			if (request == typeof(short))
			{
				return (short) this.GetNextRandom();
			}
			if (request == typeof(int))
			{
				return (int) this.GetNextRandom();
			}
			if (request == typeof(long))
			{
				return this.GetNextRandom();
			}
			if (request == typeof(sbyte))
			{
				return (sbyte) this.GetNextRandom();
			}
			if (request == typeof(float))
			{
				return (float) this.GetNextRandom();
			}
			if (request == typeof(ushort))
			{
				return (ushort) this.GetNextRandom();
			}
			if (request == typeof(uint))
			{
				return (uint) this.GetNextRandom();
			}
			if (request == typeof(ulong))
			{
				return (ulong) this.GetNextRandom();
			}
			return No.Value;
		}

		private long GetNextRandom()
		{
			lock (this.syncRoot)
			{
				this.EvaluateRange();

				long result;
				do
				{
					if (this.lower >= int.MinValue &&
					    this.upper <= int.MaxValue)
					{
						result = this.random.Next((int)this.lower, (int)this.upper);
					}
					else
					{
						result = this.GetNextInt64InRange();
					}
				}
				while (this.numbers.Contains(result));

				this.numbers.Add(result);
				return result;
			}
		}

		private void EvaluateRange()
		{
			if (this.count == (this.upper - this.lower))
			{
				this.count = 0;
				this.CreateRange();
			}

			this.count++;
		}

		private void CreateRange()
		{
			var remaining = this.limits.Where(x => x > this.upper - 1).ToArray();
			if (remaining.Any() && this.numbers.Any())
			{
				this.lower = this.upper;
				this.upper = remaining.Min() + 1;
			}
			else
			{
				this.lower = limits[0];
				this.upper = this.GetUpperRangeFromLimits();
			}

			this.numbers.Clear();
		}

		/// <summary>
		/// Returns upper limit + 1 when expecting to use upper as max value in Random.Next(Int32,Int32).
		/// This ensures that the upper limit is included in the possible values returned by Random.Next(Int32,Int32)
		/// 
		/// When not expecting to use Random.Next(Int32,Int32).  It returns the original upper limit.
		/// </summary>
		/// <returns></returns>
		private long GetUpperRangeFromLimits()
		{
			return limits[1] >= Int32.MaxValue
				? limits[1]
				: limits[1] + 1;
		}

		private long GetNextInt64InRange()
		{
			var range = (ulong)(this.upper - this.lower);
			ulong limit = ulong.MaxValue - ulong.MaxValue % range;
			ulong number;
			do
			{
				var buffer = new byte[sizeof(ulong)];
				this.random.NextBytes(buffer);
				number = BitConverter.ToUInt64(buffer, 0);
			} while (number > limit);
			return (long)(number % range + (ulong)this.lower);
		}
	}
}