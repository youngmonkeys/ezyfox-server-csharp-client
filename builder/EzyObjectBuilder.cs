using System;
using System.Collections.Generic;
using com.tvd12.ezyfoxserver.client.entity;

namespace com.tvd12.ezyfoxserver.client.builder
{
	public class EzyObjectBuilder : EzyBuilder<EzyObject>
	{
		protected EzyObject product;

		public EzyObjectBuilder()
		{
			this.product = newProduct();
		}

		public EzyObjectBuilder append(Object key, Object value)
		{
			product.put(key, value);
			return this;
		}

		public EzyObjectBuilder append<K, V>(IDictionary<K, V> dict)
		{
			product.putAll<K, V>(dict);
			return this;
		}

		public EzyObject build()
		{
			return product;
		}

		protected EzyObject newProduct()
		{
			return new EzyObject();
		}
	}
}
