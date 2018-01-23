using System;
using System.Collections.Generic;
using com.tvd12.ezyfoxserver.client.entity;

namespace com.tvd12.ezyfoxserver.client.builder
{
	public class EzyArrayBuilder : EzyBuilder<EzyArray>
	{
		protected EzyArray product;

		public EzyArrayBuilder()
		{
			this.product = newProduct();
		}

		public EzyArrayBuilder append(Object value)
		{
			product.add(value);
			return this;
		}

		public EzyArrayBuilder append<T>(EzyBuilder<T> builder)
		{
			product.add(builder);
			return this;
		}

		public EzyArrayBuilder append<T>(ICollection<T> values)
		{
			product.addAll<T>(values);
			return this;
		}

		public EzyArray build()
		{
			return product;
		}

		protected EzyArray newProduct()
		{
			return new EzyArray();
		}

	}
}
