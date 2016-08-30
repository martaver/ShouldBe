using System;
using System.Runtime.Serialization;

namespace ShouldBeAssertions
{
	[Serializable]
	public class ShouldBeException : ApplicationException
	{
		public ShouldBeException(string message) : base(message) {}
		protected ShouldBeException(SerializationInfo info, StreamingContext context) : base(info, context) {}
	}
}