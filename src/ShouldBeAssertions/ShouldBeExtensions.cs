using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using KellermanSoftware.CompareNetObjects;
using KellermanSoftware.CompareNetObjects.TypeComparers;
using ShouldBeAssertions.Tools;

namespace ShouldBeAssertions
{
	public static class ShouldBeExtensions
	{
		/// <summary>
		/// Compares objects of the same type. The comparison is run on all properties, excluding properties nominated.
		/// </summary>
		/// <typeparam name="T"></typeparam>		
		/// <returns></returns>
		public static CompareLogic GetComparer<T>(bool ignoreDefaults, params Expression<Func<T, object>>[] except)
		{
			return GetComparer(ignoreDefaults, except?.Select(PropertyInfos.Get).Select(i => i.Name).ToArray());
		}

		public static CompareLogic GetComparer<T, TActual>(bool ignoreDefaults, params Expression<Func<T, object>>[] except)
		{
			return GetComparer<T, TActual>(ignoreDefaults, except?.Select(PropertyInfos.Get).Select(i => i.Name).ToArray());
		}

		/// <summary>
		/// Compares objects of two different types. The comparison is run on properties with matching names - other properties are ignored.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <typeparam name="TActual"></typeparam>
		/// <returns></returns>
		public static CompareLogic GetComparer<T, TActual>(bool ignoreDefaults, params string[] except)
		{
			return new CompareLogic
			{
				Config =
				{
					ActualName = typeof (TActual).Name,
					MaxDifferences = 100,
					ExpectedName = typeof (T).Name,
					IgnoreObjectTypes = true,
					CompareProperties = true,
					MembersToIgnore = except?.ToList(),
					CustomComparers = new List<BaseTypeComparer>
					{
						new ConstraintComparer(ignoreDefaults, RootComparerFactory.GetRootComparer())
					}
				}
			};
		}

		public static CompareLogic GetComparer(bool ignoreDefaults, params string[] except)
		{
			return new CompareLogic
			{
				Config =
				{
					ActualName = "Right",
					ExpectedName = "Left",
					MaxDifferences = 100,
					MembersToIgnore = except?.ToList(),
					CompareProperties = true,
					IgnoreObjectTypes = true,										
					CustomComparers = new List<BaseTypeComparer>
					{
						new ConstraintComparer(ignoreDefaults, RootComparerFactory.GetRootComparer())
					}
				}
			};
		}

		//		[DebuggerStepThrough]
		public static void ShouldLookLike<T>(this T left, T right)
		{
			ShouldLookLike(left, right, GetComparer(false));
		}

		public static void ShouldLookLike<T>(this T left, T right, params Expression<Func<T, object>>[] except)
		{
			ShouldLookLike(left, right, GetComparer(false, except));
		}

		public static void ShouldLookLike<T, TOther>(this T left, TOther right, params Expression<Func<T, object>>[] except)
		{
			ShouldLookLike(left, right, GetComparer<T, TOther>(false, except));
		}

		public static void ShouldLookLike(this object left, object right, params string[] except)
		{
			ShouldLookLike(left, right, GetComparer(false, except));
		}

		public static void ShouldLookLike(this object left, object right)
		{
			ShouldLookLike(left, right, GetComparer(false));
		}

		public static void ShouldAllLookLike<T>(this IEnumerable<T> left, T right)
		{
			foreach (var value in left)
			{
				ShouldLookLike(value, right, GetComparer(false));
			}
		}

		public static void ShouldAllLookLike<T>(this IEnumerable<T> left, T right, params Expression<Func<T, object>>[] except)
		{
			foreach (var value in left)
			{
				ShouldLookLike(value, right, GetComparer(false, except));
			}
		}

		public static void ShouldAllLookLike<T, TOther>(this IEnumerable<T> left, TOther right, params Expression<Func<T, object>>[] except)
		{
			foreach (var value in left)
			{
				ShouldLookLike(value, right, GetComparer<T, TOther>(false, except));
			}
		}

		public static void ShouldAllLookLike(this IEnumerable left, object right, params string[] except)
		{
			foreach (var value in left)
			{
				ShouldLookLike(value, right, GetComparer(false, except));
			}
		}

		public static void ShouldAllLookLike(this IEnumerable left, object right)
		{
			foreach (var value in left)
			{
				ShouldLookLike(value, right, GetComparer(false));
			}
		}

		//[DebuggerStepThrough]
		public static void ShouldLookLike(this object left, object right, CompareLogic compare)
		{
			var comparison = compare.Compare(left, right);
			ShouldBe.LazyConstraints.ForEach(c => c());
			if(!comparison.AreEqual) throw new ShouldBeException(comparison.DifferencesString);
			ShouldBe.LazyConstraints.Clear();
		}

		public static void ShouldHaveValuesOf<T>(this T left, T right)
		{
			ShouldHaveValuesOf(left, right, GetComparer(true));
		}

		public static void ShouldHaveValuesOf<T, TOther>(this T left, TOther right, params Expression<Func<T, object>>[] except)
		{
			ShouldHaveValuesOf(left, right, GetComparer<T, TOther>(true, except));
		}

		public static void ShouldHaveValuesOf(this object left, object right)
		{
			ShouldHaveValuesOf(left, right, GetComparer(true));
		}

		//[DebuggerStepThrough]
		private static void ShouldHaveValuesOf(this object left, object right, CompareLogic compare)
		{
			var comparison = compare.Compare(left, right);
			ShouldBe.LazyConstraints.ForEach(c => c());
			if (!comparison.AreEqual) throw new ShouldBeException(comparison.DifferencesString);
			ShouldBe.LazyConstraints.Clear();
		}

		public static void ShouldAllHave<T>(this IEnumerable<T> left, T right)
		{
			foreach (var value in left)
			{
				ShouldHaveValuesOf(value, right, GetComparer(true));
			}
		}

		public static void ShouldAllHave<T, TOther>(this IEnumerable<T> left, TOther right, params Expression<Func<T, object>>[] except)
		{
			foreach (var value in left)
			{
				ShouldHaveValuesOf(value, right, GetComparer<T, TOther>(true, except));
			}
		}

		public static void ShouldAllHave(this IEnumerable left, object right)
		{
			foreach (var value in left)
			{
				ShouldHaveValuesOf(value, right, GetComparer(true));
			}
		}
	}
}