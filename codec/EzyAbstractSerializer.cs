using System;
using System.Collections.Generic;
using com.tvd12.ezyfoxserver.client.function;

namespace com.tvd12.ezyfoxserver.client.codec
{
	public abstract class EzyAbstractSerializer<T> : EzyMessageSerializer
	{
		protected Dictionary<Type, EzyParser<Object, Object>> parsers;

		public EzyAbstractSerializer()
		{
            this.parsers = defaultParsers();
		}

		public abstract byte[] serialize(Object value);

		protected T parseNotNull(Object value)
		{
			EzyParser<Object, Object> parser = getParser(value.GetType());
			if (parser != null)
				return parseWithParser(parser, value);
			return parseWithNoParser(value);

		}

		protected T parseWithParser(EzyParser<Object, Object> parser, Object value)
		{
			return (T)parser(value);
		}

		protected T parseWithNoParser(Object value)
		{
			throw new ArgumentException("has no parse for " + value.GetType());
		}

		protected abstract T parseNil();

		protected EzyParser<Object, Object> getParser(Type type)
		{
			return parsers[type];
		}

		protected Dictionary<Type, EzyParser<Object, Object>> defaultParsers()
		{
			var parsers = new Dictionary<Type, EzyParser<Object, Object>>();
			addParsers(parsers);
			return parsers;
		}

		protected abstract void addParsers(Dictionary<Type, EzyParser<Object, Object>> parsers);
	 
	} 
}
