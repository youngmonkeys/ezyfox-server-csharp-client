using System;
using com.tvd12.ezyfoxserver.client.builder;
using com.tvd12.ezyfoxserver.client.entity;

namespace com.tvd12.ezyfoxserver.client.factory
{
	public class EzyEntityFactory
	{
		private static readonly EzyEntityBuilderCreator CREATOR 
				= new EzyEntityBuilderCreator();

		public static readonly EzyArray EMPTY_ARRAY = newArray();

		public static T create<T>()
		{
			return CREATOR.create<T>();
		}

		public static EzyObject newObject()
		{
			return CREATOR.newObject();
		}

		public static EzyArray newArray()
		{
			return CREATOR.newArray();
		}

		public static EzyObjectBuilder newObjectBuilder()
		{
			return CREATOR.newObjectBuilder();
		}

		public static EzyArrayBuilder newArrayBuilder()
		{
			return CREATOR.newArrayBuilder();
		}
	}
}
