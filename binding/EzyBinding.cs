using System;
using System.Collections.Generic;
using com.tvd12.ezyfoxserver.client.entity;

namespace com.tvd12.ezyfoxserver.client.binding
{
    public class EzyBinding
    {
        private readonly Ezymarshaller marshaller;
        private readonly EzyUnmarshaller unmarshaller;

        public EzyBinding(
            IDictionary<Type, IEzyWriter> writerByInType,
            IDictionary<Type, IEzyReader> readerByOutType)
        {
            this.marshaller = new Ezymarshaller(writerByInType);
            this.unmarshaller = new EzyUnmarshaller(readerByOutType);
        }

        public static EzyBindingBuilder builder()
        {
            return new EzyBindingBuilder();
        }

        public T marshall<T>(object input)
        {
            return marshaller.marshall<T>(input);
        }

        public T unmarshall<T>(object input)
        {
            return unmarshaller.unmarshall<T>(input);
        }
    }

    public class EzyBindingBuilder
    {
        private readonly IDictionary<Type, IEzyWriter> writerByInType;
        private readonly IDictionary<Type, IEzyReader> readerByOutType;

        public EzyBindingBuilder()
        {
            this.writerByInType = new Dictionary<Type, IEzyWriter>();
            this.readerByOutType = new Dictionary<Type, IEzyReader>();
        }

        public EzyBindingBuilder addReader(IEzyReader reader)
        {
            this.readerByOutType[reader.getOutType()] = reader;
            return this;
        }

        public EzyBindingBuilder addWriter(IEzyWriter writer)
        {
            this.writerByInType[writer.getInType()] = writer;
            return this;
        }

        public EzyBindingBuilder addConverter(IEzyConverter converter)
        {
            this.readerByOutType[converter.getOutType()] = converter;
            this.writerByInType[converter.getInType()] = converter;
            return this;
        }

        public EzyBindingBuilder addReflectionMapConverter<T>()
        {
            return addConverter(new EzyReflectionMapConverter<T>());
        }

        public EzyBinding build()
        {
            return new EzyBinding(writerByInType, readerByOutType);
        }
    }

    public class Ezymarshaller
    {
        private readonly IDictionary<Type, IEzyWriter> writerByInType;

        public Ezymarshaller(IDictionary<Type, IEzyWriter> writerByInType)
        {
            this.writerByInType = writerByInType;
        }

        public T marshall<T>(object input)
        {
            return (T)marshallByInType(input, input.GetType());
        }

        public object marshallByInType(object input, Type inType)
        {
            if (input == null)
            {
                return null;
            }
            if (writerByInType.ContainsKey(inType))
            {
                IEzyWriter writer = writerByInType[inType];
                return writer.write(input, this);
            }
            return input;
        }
    }

    public class EzyUnmarshaller
    {
        private readonly IDictionary<Type, IEzyReader> readerByOutType;

        public EzyUnmarshaller(IDictionary<Type, IEzyReader> readerByOutType)
        {
            this.readerByOutType = readerByOutType;
        }

        public T unmarshall<T>(object input)
        {
            return (T)unmarshallByOutType(input, typeof(T));
        }

        public object unmarshallByOutType(object input, Type outType)
        {
            if (input == null)
            {
                return null;
            }
            if (readerByOutType.ContainsKey(outType))
            {
                IEzyReader reader = readerByOutType[outType];
                return reader.read(input, this);
            }
            return input;
        }

        public IList<T> unmarshallList<T>(EzyArray array)
        {
            List<T> answer = new List<T>();
            for (int i = 0; i < array.size(); ++i)
            {
                answer.Add(unmarshall<T>(array.get<object>(i)));
            }
            return answer;
        }
    }
}
