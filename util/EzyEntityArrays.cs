using System;
using com.tvd12.ezyfoxserver.client.entity;
using com.tvd12.ezyfoxserver.client.factory;
using com.tvd12.ezyfoxserver.client.builder;

namespace com.tvd12.ezyfoxserver.client.util
{
	public sealed class EzyEntityArrays
	{
		private EzyEntityArrays()
		{
		}

		public static EzyArray newArray()
		{
			EzyArray array = newArrayBuilder().build();
			return array;
		}

		public static EzyArray newArray<T>(params T[] args)
		{
			EzyArray array = newArrayBuilder().append(args).build();
			return array;
		}

		private static EzyArrayBuilder newArrayBuilder()
		{
			return EzyEntityFactory.newArrayBuilder();
		}
	}

}
