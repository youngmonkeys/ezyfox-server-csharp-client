using System;
using System.Text;
using System.Collections.Generic;
namespace com.tvd12.ezyfoxserver.client.entity
{
	public class EzyArray : EzyRoArray
	{
		protected readonly List<Object> list = new List<Object>();

		public void clear()
		{
			list.Clear();
		}

		public void add(Object value)
		{
			list.Add(value);
		}

		public int size()
		{
			return list.Count;
		}


		public bool isEmpty()
		{
			return size() == 0;
		}

		public T get<T>(int index)
		{
			return (T)list[index];
		}

		public List<T> toList<T>()
		{
			List<T> answer = new List<T>();
			foreach(Object item in list) 
			{
				answer.Add((T)item);
			}
			return answer;
		}

		public object Clone()
		{
			var answer = new EzyArray();
			foreach (Object item in list)
			{
				if (item is ICloneable)
				{
					answer.add(((ICloneable)item).Clone());
				}
			}
			return answer;
		}

		public EzyArray duplicate<EzyArray>()
		{
			return (EzyArray)Clone();
		}

		public override string ToString()
		{
			return new StringBuilder()
				.Append("[")
				.Append(String.Join(",", list))
				.Append("]")
				.ToString();
		}
	}
}
