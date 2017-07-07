using System;
using System.Globalization;

namespace ShouldBeAssertions.Core.Tools
{
	/// <summary>
	/// Creates a sequence of random printable ASCII characters (Dec 33-126).
	/// </summary>
	public class RandomTextGenerator
	{
		private readonly RandomNumberGenerator randomPrintableCharNumbers;

		/// <summary>
		/// Initializes a new instance of the
		/// <see cref="RandomTextGenerator"/> class.
		/// </summary>
		public RandomTextGenerator()
		{
			this.randomPrintableCharNumbers =
				new RandomNumberGenerator(33, 126);
		}

		/// <summary>
		/// Creates a new specimen based on a request.
		/// </summary>
		/// <param name="request">The request that describes what to create.
		/// </param>
		/// <param name="context">A context that can be used to create other 
		/// specimens.</param>
		/// <returns>
		/// The requested specimen if possible; otherwise a 
		/// <see cref="NoSpecimen"/> instance.
		/// </returns>
		public object Create(Type type)
		{
			if (type == typeof(string))
			{
				return Guid.NewGuid().ToString();
			}

			if (type == typeof(char))
			{
				return Convert.ToChar(this.randomPrintableCharNumbers.Create(typeof(int)), CultureInfo.CurrentCulture);
			}

			return No.Value;
		}
	}
}