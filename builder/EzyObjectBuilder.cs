using System;
using com.tvd12.ezyfoxserver.client.entity;

namespace com.tvd12.ezyfoxserver.client.builder
{
	public class EzyObjectBuilder
	{
		protected EzyObject product;

		public EzyObjectBuilder()
		{
			this.product = newProduct();
		}

		protected EzyObject newProduct()
		{
			return product;
		}
	}
}
