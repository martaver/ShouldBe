using System;
using KellermanSoftware.CompareNetObjects;

namespace ShouldBeAssertions
{
	public class ConstraintResolver
	{
		public ConstraintResolver(object placeholder)
		{
			Placeholder = placeholder;
		}

		public object Placeholder { get; }

		public Func<ConstraintComparer, CompareParms, bool, bool?> Compare { get; set; }
	}
}