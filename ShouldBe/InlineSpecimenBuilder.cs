using System;
using System.Reflection;
using Ploeh.AutoFixture.Kernel;

namespace ShouldBe
{
	/// <summary>
	/// Lets us set up a Type or Property customization for AutoFixture with a lambda.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public class InlineSpecimenBuilder<T> : SpecimenBuilder<T>
	{
		private readonly Func<PropertyInfo, ISpecimenContext, T> _builder;

		public InlineSpecimenBuilder(Func<PropertyInfo, ISpecimenContext, T> builder)
		{
			_builder = builder;
		}

		public override T Build(PropertyInfo propertyInfo, ISpecimenContext context)
		{
			return _builder(propertyInfo, context);
		}
	}
}