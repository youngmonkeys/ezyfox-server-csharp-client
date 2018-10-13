using System;
using System.Collections.Generic;
using com.tvd12.ezyfoxserver.client.entity;
using com.tvd12.ezyfoxserver.client.factory;
using com.tvd12.ezyfoxserver.client.builder;

namespace com.tvd12.ezyfoxserver.client.util
{
	public sealed class EzyEntityObjects
	{
		private EzyEntityObjects()
		{
		}

		public static EzyObject newObject()
		{
			return newObjectBuilder().build();
		}

		public static EzyObject newObject<K, V>(IDictionary<K, V> dict)
		{
			EzyObject obj = newObjectBuilder().append(dict).build();
			return obj;
		}

		private static EzyObjectBuilder newObjectBuilder()
		{
			return EzyEntityFactory.newObjectBuilder();
		}
	}
}
