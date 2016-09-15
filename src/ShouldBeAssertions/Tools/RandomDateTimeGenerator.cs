using System;

namespace ShouldBeAssertions.Tools
{
	/// <summary>
	/// Creates random <see cref="DateTime"/> specimens.
	/// </summary>
	/// <remarks>
	/// The generated <see cref="DateTime"/> values will be within
	/// a range of ± two years from today's date,
	/// unless a different range has been specified in the constructor.
	/// </remarks>
	public class RandomDateTimeGenerator
	{
		private readonly RandomNumberGenerator _randomizer;

		/// <summary>
		/// Initializes a new instance of the <see cref="RandomDateTimeGenerator"/> class.
		/// </summary>
		public RandomDateTimeGenerator() : this(DateTime.MinValue, DateTime.MaxValue) { }

		/// <summary>
		/// Initializes a new instance of the <see cref="RandomDateTimeGenerator"/> class
		/// for a specific range of dates.
		/// </summary>
		/// <param name="minDate">The lower bound of the date range.</param>
		/// <param name="maxDate">The upper bound of the date range.</param>
		/// <exception cref="ArgumentException">
		/// <paramref name="minDate"/> is greater than <paramref name="maxDate"/>.
		/// </exception>
		public RandomDateTimeGenerator(DateTime minDate, DateTime maxDate)
		{
			if (minDate >= maxDate)
			{
				throw new ArgumentException("The 'minDate' argument must be less than the 'maxDate'.");
			}

			this._randomizer = new RandomNumberGenerator(minDate.Ticks, maxDate.Ticks);
		}		
		
		public object Create(Type type)
		{
			switch (Type.GetTypeCode(type))
			{				
				case TypeCode.DateTime:
					return new DateTime(GetRandomNumberOfTicks());
				default:
					return No.Value;
			}
		}

		private long GetRandomNumberOfTicks()
		{
			return (long)_randomizer.Create(typeof(long));
		}
	}
}