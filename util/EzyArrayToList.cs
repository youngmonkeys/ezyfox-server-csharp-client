using System.Collections.Generic;
using com.tvd12.ezyfoxserver.client.entity;

namespace com.tvd12.ezyfoxserver.client.util
{
	public sealed class EzyArrayToList
	{

		private static readonly EzyArrayToList INSTANCE = new EzyArrayToList();

		private EzyArrayToList()
		{
		}

		public static EzyArrayToList getInstance()
		{
			return INSTANCE;
		}

		public List<T> toList<T>(EzyArray array)
		{
			List<T> answer = new List<T>();
			for (int i = 0; i < array.size(); ++i)
			{
				object item = array.get<object>(i);
				object sitem = item;
				if (item != null)
				{
					EzyObjectToMap objectToMap = EzyObjectToMap.getInstance();
					if (item is EzyObject)
					{
						sitem = objectToMap.toMap<object, object>((EzyObject)item);
					}
					else if (item is EzyArray)
					{
						sitem = toList<object>((EzyArray)item);
					}
				}
				answer.Add((T)sitem);
		}
		return answer;
	}
	
}

}
