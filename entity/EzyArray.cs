using System;
using System.Text;
using System.Collections.Generic;
using com.tvd12.ezyfoxserver.client.io;
using com.tvd12.ezyfoxserver.client.builder;

namespace com.tvd12.ezyfoxserver.client.entity
{
	public class EzyArray : EzyRoArray
	{
		protected readonly List<Object> list;
		protected readonly EzyOutputTransformer outputTransformer;

		public EzyArray(EzyOutputTransformer outputTransformer)
		{
			this.outputTransformer = outputTransformer;
			this.list = new List<Object>();
		}

		public void clear()
		{
			list.Clear();
		}

		public void add(Object value)
		{
			list.Add(value);
		}

		public void add<T>(EzyBuilder<T> builder)
		{
			T t = builder.build();
			list.Add(t);
		}

		public void addAll<T>(ICollection<T> values)
		{
			foreach (T value in values)
			{
				list.Add(value);
			}
		}

		public int size()
		{
			int s = list.Count;
			return s;
		}

		public bool isEmpty()
		{
			bool empty = (size() == 0);
			return empty;
		}

		public T get<T>(int index)
		{
			var answer = list[index];
			if (outputTransformer == null)
			{
				return (T)answer;
			}
			T t = outputTransformer.transform<T>(answer);
			return t;

		}

		public T get<T>(int index, T defValue)
		{
			int count = list.Count;
			if (index >= count)
				return defValue;
			T t = get<T>(index);
			return t;
		}

		public List<T> toList<T>()
		{
			List<T> answer = new List<T>();
			foreach (Object item in list)
			{
				answer.Add((T)item);
			}
			return answer;
		}

		public object Clone()
		{
			var answer = new EzyArray(outputTransformer);
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
			EzyArray c = (EzyArray)Clone();
			return c;
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
