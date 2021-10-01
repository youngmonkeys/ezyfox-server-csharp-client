using System;
using System.Text;
using System.Collections.Generic;
using com.tvd12.ezyfoxserver.client.io;
using com.tvd12.ezyfoxserver.client.util;
using com.tvd12.ezyfoxserver.client.builder;

namespace com.tvd12.ezyfoxserver.client.entity
{
	public class EzyArray : EzyRoArray
	{
		protected readonly List<Object> list;
		protected readonly EzyInputTransformer inputTransformer;
		protected readonly EzyOutputTransformer outputTransformer;

		public EzyArray(
			EzyInputTransformer inputTransformer,
			EzyOutputTransformer outputTransformer)
		{
			this.inputTransformer = inputTransformer;
			this.outputTransformer = outputTransformer;
			this.list = new List<Object>();
		}

		public void clear()
		{
			list.Clear();
		}

		public void add(Object value)
		{
			list.Add(inputTransformer.transform(value));
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
				list.Add(inputTransformer.transform(value));
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
			EzyArrayToList arrayToList = EzyArrayToList.getInstance();
			List<T> list = arrayToList.toList<T>(this);
			return list;
		}

		public object Clone()
		{
			var answer = new EzyArray(inputTransformer, outputTransformer);
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
