using System;
using System.Collections.Generic;
using com.tvd12.ezyfoxserver.client.io;
using com.tvd12.ezyfoxserver.client.builder;
using com.tvd12.ezyfoxserver.client.constant;
using com.tvd12.ezyfoxserver.client.entity;

namespace com.tvd12.ezyfoxserver.client.factory
{
	public class EzyEntityBuilderCreator
	{
		protected readonly Dictionary<Type, Func<Object>> suppliers;
		protected static readonly EzyInputTransformer INPUT_TRANSFORMER
				= new EzyInputTransformer();
		protected static readonly EzyOutputTransformer OUTPUT_TRANSFORMER
				= new EzyOutputTransformer();

		public EzyEntityBuilderCreator()
		{
			this.suppliers = defaultSuppliers();
		}

		public T create<T>()
		{
			Type type = typeof(T);
			Func<Object> supplier = suppliers[type];
			if (supplier != null)
			{
				return (T)supplier();
			}
			throw new ArgumentException("cannot create builder with type: " + type);
		}

		public EzyObject newObject()
		{
			EzyObject obj = new EzyObject(INPUT_TRANSFORMER, OUTPUT_TRANSFORMER);
			return obj;
		}

		public EzyArray newArray()
		{
			EzyArray array = new EzyArray(INPUT_TRANSFORMER, OUTPUT_TRANSFORMER);
			return array;
		}

		public EzyObjectBuilder newObjectBuilder()
		{
			EzyObjectBuilder builder = new EzyObjectBuilder(INPUT_TRANSFORMER, OUTPUT_TRANSFORMER);
			return builder;
		}

		public EzyArrayBuilder newArrayBuilder()
		{
			EzyArrayBuilder builder = new EzyArrayBuilder(INPUT_TRANSFORMER, OUTPUT_TRANSFORMER);
			return builder;
		}

		protected Dictionary<Type, Func<Object>> defaultSuppliers()
		{
			var answer = new Dictionary<Type, Func<Object>>();
			answer[EzyTypes.EZY_ARRAY_TYPE] = () => newArray();
			answer[EzyTypes.EZY_OBJECT_TYPE] = () => newObject();
			answer[EzyTypes.EZY_ARRAY_BUILDER_TYPE] = () => newObjectBuilder();
			answer[EzyTypes.EZY_OBJECT_BUILDER_TYPE] = () => newArrayBuilder();
			return answer;
		}
	}
}
