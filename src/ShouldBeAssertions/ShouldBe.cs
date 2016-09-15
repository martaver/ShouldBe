using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Ploeh.AutoFixture;
using ShouldBeAssertions.Tools;

namespace ShouldBeAssertions
{
	public static class ShouldBe
	{
		[ThreadStatic]
		private static Dictionary<object, ConstraintResolver> _constraints;

		[ThreadStatic]
		private static Dictionary<string, object> _namedValues;

		[ThreadStatic]
		private static List<Action> _lazyConstraints;


		internal static Dictionary<object, ConstraintResolver> Constraints => _constraints ?? (_constraints = new Dictionary<object, ConstraintResolver>());
		internal static List<Action> LazyConstraints => _lazyConstraints ?? (_lazyConstraints = new List<Action>());
		internal static Dictionary<string, object> NamedValues => _namedValues ?? (_namedValues = new Dictionary<string, object>());

		static readonly Fixture F = new Fixture();

		static ShouldBe()
		{
			F.Customizations.Add(new InlineSpecimenBuilder<int>((p, cx) => Randoms.NextInt(max: 0)));
		}

		private static T GetPlaceholder<T>()
		{
			if (typeof(T).IsValueType || typeof(T) == typeof(string))
			{
				return F.Create<T>();
			}
			return (T)FormatterServices.GetUninitializedObject(typeof(T));
		}


		public static T Any<T>(string name = null)
		{
			var ph = GetPlaceholder<T>();

			Constraints[ph] = new ConstraintResolver(ph)
			{
				Compare = (comparer, parms, isLeftConstrained) =>
				{
					if (name != null)
					{
						if (!isLeftConstrained)
						{
							NamedValues[name] = parms.Object1;
						}
						else
						{
							NamedValues[name] = parms.Object2;
						}
					}

					//Return false to indicate no difference, with no need to run further comparisons.
					return false;
				}
			};

			return ph;
		}

		public static T NonDefault<T>()
		{
			var ph = GetPlaceholder<T>();

			Constraints[ph] = new ConstraintResolver(ph)
			{
				Compare = (comparer, parms, isLeftConstrained) => Equals(default(T), isLeftConstrained ? parms.Object2 : parms.Object1)
			};

			return ph;
		}

		public static T NameThis<T>(this T value, string name)
		{
			NamedValues[name] = value;
			return value;
		}

		public static T SameAs<T>(string name)
		{
			var ph = GetPlaceholder<T>();

			Constraints[ph] = new ConstraintResolver(ph)
			{
				Compare = (comparer, parms, isLeftConstrained) =>
				{
					LazyConstraints.Add(() =>
					{
						object value;
						if (!NamedValues.TryGetValue(name, out value))
						{
							throw new InvalidOperationException($"Can't compare to property '{name}' because no property was found with that name. Designate the property to compare to by naming it '{name}' in the ShouldBe constraint's 'name' parameter or if the value is a constant, use the '{nameof(NameThis)}' extension method.");
						}

						if (!isLeftConstrained)
						{
							parms.Object2 = value;
							parms.Object2Type = value.GetType();
							comparer.PassthroughComparison(parms);
						}
						else
						{
							parms.Object1 = value;
							parms.Object1Type = value.GetType();
							comparer.PassthroughComparison(parms);
						}
					});
					return false;
				}
			};

			return ph;
		}

		public static T AllowedWhen<T>(Func<T, T, bool> predicate)
		{
			var ph = GetPlaceholder<T>();
			Constraints[ph] = new ConstraintResolver(ph);
			return ph;
		}

		public static T OneOf<T>(params T[] values)
		{
			var ph = GetPlaceholder<T>();
			Constraints[ph] = new ConstraintResolver(ph);
			return ph;
		}

		public static int InRange(int min = Int32.MinValue, int max = Int32.MaxValue)
		{
			var ph = Randoms.NextInt();
			Constraints[ph] = new ConstraintResolver(ph);
			return ph;
		}

		public static int GreaterThan(int min = Int32.MinValue)
		{
			return InRange(min);
		}

		public static int LessThan(int max = Int32.MaxValue)
		{
			return InRange(max: max);
		}
	}
}