using System;
using System.Collections.Generic;

namespace com.tvd12.ezyfoxserver.client.io
{
	public sealed class EzyLists
	{
		private EzyLists()
		{
		}

		public static IList<V> clone<V>(ICollection<V> values)
		{
			var answer = new List<V>();
			foreach (var v in values)
			{
				answer.Add(v);
			}
			return answer;
		}

	}
}
