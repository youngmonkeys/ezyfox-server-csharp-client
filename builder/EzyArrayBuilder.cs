using System;
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
