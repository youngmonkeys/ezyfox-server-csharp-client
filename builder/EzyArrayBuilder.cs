using System;
using System.Collections;
using System.Collections.Generic;
using com.tvd12.ezyfoxserver.client.io;
using com.tvd12.ezyfoxserver.client.entity;

namespace com.tvd12.ezyfoxserver.client.builder
{
	public class EzyArrayBuilder : EzyBuilder<EzyArray>
	{
		protected EzyArray product;
		protected EzyInputTransformer inputTransformer;
		protected EzyOutputTransformer outputTransformer;

		public EzyArrayBuilder(
			EzyInputTransformer inputTransformer,
			EzyOutputTransformer outputTransformer)
		{
			this.inputTransformer = inputTransformer;
			this.outputTransformer = outputTransformer;
            this.product = newProduct();
		}

		public EzyArrayBuilder append(Object value)
		{
			product.add(value);
			return this;
		}

        public EzyArrayBuilder append(byte[] value)
        {
            product.add(value);
            return this;
        }

        public EzyArrayBuilder append<T>(EzyBuilder<T> builder)
		{
			product.add(builder);
			return this;
		}

		public EzyArrayBuilder append<T>(params T[] values)
		{
			foreach (T v in values)
			{
				product.add(v);
			}
			return this;
		}

		public EzyArrayBuilder appendRawList(IList values)
		{
			product.addRawList(values);
			return this;
		}

		public EzyArrayBuilder appendAll<T>(IList<T> values)
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
			return new EzyArray(inputTransformer, outputTransformer);
		}
	}
}
