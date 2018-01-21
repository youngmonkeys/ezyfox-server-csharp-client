using System;

namespace com.tvd12.ezyfoxserver.client.factory
{
	public class EzyEntityFactory
	{
		private static readonly EzyEntityBuilderCreator CREATOR 
				= new EzyEntityBuilderCreator();

		public static T create<T>()
		{
			return CREATOR.create<T>();
		}
	}
}
