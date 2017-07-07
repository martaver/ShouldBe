using System;

namespace ShouldBeAssertions.Core
{
	public class ShouldBeException : Exception
	{
		public ShouldBeException(string message) : base(message) {}
	}
}