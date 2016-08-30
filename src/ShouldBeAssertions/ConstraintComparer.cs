using System;
using KellermanSoftware.CompareNetObjects;
using KellermanSoftware.CompareNetObjects.TypeComparers;

namespace ShouldBeAssertions
{
	public class ConstraintComparer : BaseTypeComparer
	{
		private readonly bool _ignoreDefaults;
		private bool _isComparing;

		public ConstraintComparer(bool ignoreDefaults, RootComparer rootComparer) : base(rootComparer)
		{
			_ignoreDefaults = ignoreDefaults;
		}

		public override bool IsTypeMatch(Type type1, Type type2)
		{
			var isMatch = !_isComparing;
			_isComparing = false;
			return isMatch;
		}

		public object GetDefault(CompareParms parms)
		{
			var type = (parms.Object1 ?? parms.Object2)?.GetType();
			if (type == null) return null;

			return type.IsValueType ? Activator.CreateInstance(type) : null;
		}

		public override void CompareType(CompareParms parms)
		{
			ConstraintResolver resolverFromObject1 = null;
			ConstraintResolver resolverFromObject2 = null;

			//If we are constrained, do nothing.
			var object1IsConstrained = parms.Object1 != null && ShouldBe.Constraints.TryGetValue(parms.Object1, out resolverFromObject1);
			var object2IsConstrained = parms.Object2 != null && ShouldBe.Constraints.TryGetValue(parms.Object2, out resolverFromObject2);

			var isConstrained = object1IsConstrained || object2IsConstrained;

			var isLeftConstrained = object1IsConstrained;

			if (_ignoreDefaults && (parms.Object2?.Equals(GetDefault(parms)) ?? true))
			{
				return;
			}

			var resolver = resolverFromObject1 ?? resolverFromObject2;

			if (isConstrained && resolver?.Compare != null)
			{
				var hasDifference = resolver.Compare(this, parms, isLeftConstrained);
				if (!hasDifference.HasValue) PassthroughComparison(parms);
				else if (hasDifference.Value)
				{
					AddDifference(parms);
				}
				return;
			}

			PassthroughComparison(parms);
		}

		public void PassthroughComparison(CompareParms parms)
		{
			_isComparing = true;
			RootComparer.Compare(parms);
		}
	}
}