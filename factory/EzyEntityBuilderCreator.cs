using System;
using System.Collections.Generic;
using com.tvd12.ezyfoxserver.client.builder;
using com.tvd12.ezyfoxserver.client.constant;

namespace com.tvd12.ezyfoxserver.client.factory
{
	public class EzyEntityBuilderCreator
	{
		protected readonly Dictionary<Type, Func<Object>> suppliers;

		public EzyEntityBuilderCreator()
		{
			this.suppliers = defaultSuppliers();
		}

		public T create<T>()
		{
			Type type = typeof(T);
			var supplier = suppliers[type];
			if (supplier != null)
			{
				return (T)supplier();
			}
			throw new ArgumentException("cannot create builder with type: " + type);
		}

		protected Dictionary<Type, Func<Object>> defaultSuppliers()
		{
			var answer = new Dictionary<Type, Func<Object>>();
			answer[EzyTypes.EZY_ARRAY_BUILDER_TYPE] = () => new EzyArrayBuilder();
			answer[EzyTypes.EZY_OBJECT_BUILDER_TYPE] = () => new EzyObjectBuilder();
			return answer;
		}
	}
}
