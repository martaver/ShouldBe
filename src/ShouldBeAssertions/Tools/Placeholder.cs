using System;
using System.Runtime.Serialization;

namespace ShouldBeAssertions.Tools
{
	public static class Placeholder
	{
		private static readonly RandomNumberGenerator Number = new RandomNumberGenerator();
		private static readonly RandomDateTimeGenerator Date = new RandomDateTimeGenerator();
		private static readonly RandomTextGenerator Text = new RandomTextGenerator();

		public static bool IsGeneratableValueType<T>()
		{
			return typeof(T).IsValueType || typeof(T) == typeof(string);
		}

		public static bool IsNullableValueType(Type type)
		{
			return type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>);
		}

		public static T Create<T>()
		{
			var type = typeof(T);

			if (IsNullableValueType(type)) type = Nullable.GetUnderlyingType(type);

			if (IsGeneratableValueType<T>())
			{				
				var value = Number.Create(type);
				if (value != No.Value) return (T)value;

				value = Date.Create(type);
				if (value != No.Value) return (T)value;

				value = Text.Create(type);
				if (value != No.Value) return (T)value;
			}

			return (T)FormatterServices.GetUninitializedObject(typeof(T));
		}	
	}
}
