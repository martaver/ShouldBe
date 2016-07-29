using System;
using System.Reflection;
using Ploeh.AutoFixture.Kernel;

namespace ShouldBe
{
	public abstract class SpecimenBuilder<T> : ISpecimenBuilder
	{
		public object Create(object request, ISpecimenContext context)
		{
			var propertyInfo = request as PropertyInfo;
			if (propertyInfo != null && propertyInfo.PropertyType == typeof(T))
			{
				return Build(propertyInfo, context);
			}

			var type = request as Type;
			if (type != null && type == typeof(T))
			{
				return Build(null, context);
			}

			return new NoSpecimen(request);
		}

		/// <summary>
		/// Build the specimen type. Use ISpecimenContext to populate properties with fixture data, e.g. 'context.Create&lt;string&gt;()'
		/// </summary>
		/// <param name="propertyInfo"></param>
		/// <param name="context"></param>
		/// <returns></returns>
		public abstract T Build(PropertyInfo propertyInfo, ISpecimenContext context);
	}
}