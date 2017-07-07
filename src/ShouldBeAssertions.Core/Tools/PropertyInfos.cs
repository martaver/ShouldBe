using System;
using System.Linq.Expressions;
using System.Reflection;

namespace ShouldBeAssertions.Core.Tools
{
	/// <summary>
	/// Contains common helper methods for Property manipulation.
	/// </summary>
	public static class PropertyInfos
	{
		public static Func<object, object> GetGetter(this PropertyInfo info)
		{
			var pr = Expression.Parameter(typeof(object), "i");

			var equalsNull = Expression.Equal(pr, Expression.Constant(null));

			var cast = Expression.TypeAs(pr, info.DeclaringType);
			var property = Expression.Property(cast, info);
			var convert = Expression.TypeAs(property, typeof(object));

			var ifThenElse = Expression.Condition(equalsNull, Expression.Constant(null), convert);

			return (Func<object, object>)Expression.Lambda(ifThenElse, pr).Compile();
		}

		/// <summary>
		/// Gets the PropertyInfo of the property targetted by a selector expression.
		/// </summary>
		/// <typeparam name="T">The Type to select a property from.</typeparam>
		/// <param name="selector">An Expression targetting a property on Type 'T'.</param>
		/// <returns>The PropertyInfo of the property targeted by 'selector' on Type 'T'.</returns>
		public static PropertyInfo Get<T>(Expression<Func<T, object>> selector)
		{
			return Get<T, object>(selector);
		}

		/// <summary>
		/// Gets the PropertyInfo of the property targetted by a selector expression.
		/// </summary>
		/// <typeparam name="T">The Type to select a property from.</typeparam>
		/// <param name="selector">An Expression targetting a property on Type 'T'.</param>
		/// <returns>The PropertyInfo of the property targeted by 'selector' on Type 'T'.</returns>
		public static PropertyInfo Get<T, TProperty>(Expression<Func<T, TProperty>> selector)
		{
			Expression body = selector;
			//This will be a lambda expression, so get its body.
			if (body is LambdaExpression)
			{
				body = ((LambdaExpression)body).Body;
			}
			//If this is a value type, the expression will include a 'Convert' statement. Traverse this into the MemberAccess expression.
			if (body.NodeType == ExpressionType.Convert)
			{
				body = ((UnaryExpression)body).Operand;
			}
			//Get the MemberAccess expression.
			if (body.NodeType == ExpressionType.MemberAccess)
			{
				var info = ((MemberExpression)body).Member as PropertyInfo;
				if (info == null) throw new ArgumentException("Expression 'selector' does not target a Property.");
				return info;
			}
			throw new ArgumentException("Expression 'selector' does not target a Property.");
		}
	}
}