using System;
using com.tvd12.ezyfoxserver.client.entity;

namespace com.tvd12.ezyfoxserver.client.binding
{
    public interface IEzyReader
    {
        object read(object input, EzyUnmarshaller unmarshaller);

        Type getOutType();
    }

    public interface IEzyWriter
    {
        object write(object input, Ezymarshaller marshaller);

        Type getInType();
    }

    public abstract class EzyMapToObject<T> : IEzyReader
    {
        public object read(object input, EzyUnmarshaller unmarshaller)
        {
            return mapToObject((EzyObject)input, unmarshaller);
        }

        protected abstract T mapToObject(EzyObject map, EzyUnmarshaller unmarshaller);

        public Type getOutType()
        {
            return typeof(T);
        }
    }

    public abstract class EzyArrayToObject<T> : IEzyReader
    {
        public object read(object input, EzyUnmarshaller unmarshaller)
        {
            return arrayToObject((EzyArray)input, unmarshaller);
        }

        protected abstract T arrayToObject(EzyArray array, EzyUnmarshaller unmarshaller);

        public Type getOutType()
        {
            return typeof(T);
        }
    }

    public abstract class EzyObjectToMap<T> : IEzyWriter
    {
        public object write(object input, Ezymarshaller marshaller)
        {
            return objectToMap((T)input, marshaller);
        }

        protected abstract EzyObject objectToMap(T obj, Ezymarshaller marshaller);

        public Type getInType()
        {
            return typeof(T);
        }
    }

    public abstract class EzyobjectToArray<T> : IEzyWriter
    {
        public object write(object input, Ezymarshaller marshaller)
        {
            return objectToArray((T)input, marshaller);
        }

        protected abstract EzyArray objectToArray(T obj, Ezymarshaller marshaller);

        public Type getInType()
        {
            return typeof(T);
        }
    }

    public interface IEzyConverter: IEzyReader, IEzyWriter
    {
    }

    public abstract class EzyMapConverter<T> : IEzyConverter
    {
        public object read(object input, EzyUnmarshaller unmarshaller)
        {
            return mapToObject((EzyObject)input, unmarshaller);
        }

        public object write(object input, Ezymarshaller marshaller)
        {
            return objectToMap((T)input, marshaller);
        }

        protected abstract T mapToObject(EzyObject map, EzyUnmarshaller unmarshaller);

        protected abstract EzyObject objectToMap(T obj, Ezymarshaller marshaller);

        public Type getInType()
        {
            return typeof(T);
        }

        public Type getOutType()
        {
            return typeof(T);
        }
    }

    public abstract class EzyArrayConverter<T> : IEzyConverter
    {
        public object read(object input, EzyUnmarshaller unmarshaller)
        {
            return arrayToObject((EzyArray)input, unmarshaller);
        }

        public object write(object input, Ezymarshaller marshaller)
        {
            return objectToArray((T)input, marshaller);
        }

        protected abstract T arrayToObject(EzyArray array, EzyUnmarshaller unmarshaller);

        protected abstract EzyArray objectToArray(T obj, Ezymarshaller marshaller);

        public Type getInType()
        {
            return typeof(T);
        }

        public Type getOutType()
        {
            return typeof(T);
        }
    }
}
