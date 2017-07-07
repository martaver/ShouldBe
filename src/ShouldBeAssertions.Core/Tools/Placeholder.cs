using System;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace ShouldBeAssertions.Core.Tools
{
	public static class Placeholder
	{
		private static readonly RandomNumberGenerator Number = new RandomNumberGenerator();
		private static readonly RandomDateTimeGenerator Date = new RandomDateTimeGenerator();
		private static readonly RandomTextGenerator Text = new RandomTextGenerator();

		public static bool IsGeneratableValueType<T>()
		{
			return typeof(T).GetTypeInfo().IsValueType || typeof(T) == typeof(string);
		}

		public static bool IsNullableValueType(Type type)
		{
			return type.GetTypeInfo().IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>);
		}

		public static T Create<T>()
		{
			var type = typeof(T);			

			if (IsNullableValueType(type)) type = Nullable.GetUnderlyingType(type);

			if (type == typeof(bool)) throw new NotSupportedException($"Constraints for type 'bool' are not supported, since generating a unique, random placeholder value for it is impossible. We're working on it.");

			if (IsGeneratableValueType<T>())
			{				
				var value = Number.Create(type);
				if (value != No.Value) return (T)value;

				value = Date.Create(type);
				if (value != No.Value) return (T)value;

				value = Text.Create(type);
				if (value != No.Value) return (T)value;
			}			
			var method = typeof(RuntimeHelpers).GetRuntimeMethod("GetUninitializedObject", new[] {typeof(Type)});
			return (T) method.Invoke(null, new object[] { typeof(T) });
		}	
	}
}
