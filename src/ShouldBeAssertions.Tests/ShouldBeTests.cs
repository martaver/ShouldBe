using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace ShouldBeAssertions.Tests
{
	[TestFixture]
	public class ShouldBeTests
	{
		public IEnumerable<int> GetInts(params int[] ints)
		{
			for (int i = 0; i < ints.Length; i++)
			{
				yield return ints[i];
			}
		}

		[Test]
		public void ShouldBeThrowsErrorWhenListLengthNotEqual()
		{
			var actual = GetInts(0, 1).ToList();
			actual.ShouldLookLike(new[] {0, 1, 2});
		}
	}
}