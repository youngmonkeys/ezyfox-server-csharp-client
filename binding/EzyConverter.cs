using System;
using System.Collections;
using com.tvd12.ezyfoxserver.client.entity;
using com.tvd12.ezyfoxserver.client.factory;

namespace com.tvd12.ezyfoxserver.client.binding
{
    public interface IEzyReader
    {
        object read(object input, EzyUnmarshaller unmarshaller);

        Type getOutType();
    }

    public interface IEzyWriter
    {
        object write(object input, EzyMarshaller marshaller);

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
        public object write(object input, EzyMarshaller marshaller)
        {
            return objectToMap((T)input, marshaller);
        }

        protected abstract EzyObject objectToMap(T obj, EzyMarshaller marshaller);

        public Type getInType()
        {
            return typeof(T);
        }
    }

    public abstract class EzyobjectToArray<T> : IEzyWriter
    {
        public object write(object input, EzyMarshaller marshaller)
        {
            return objectToArray((T)input, marshaller);
        }

        protected abstract EzyArray objectToArray(T obj, EzyMarshaller marshaller);

        public Type getInType()
        {
            return typeof(T);
        }
    }

    public interface IEzyConverter: IEzyReader, IEzyWriter
    {
    }

    public abstract class EzyDataConverter<T> : IEzyConverter
    {
        public object read(object input, EzyUnmarshaller unmarshaller)
        {
            return valueToData(input, unmarshaller);
        }

        public object write(object input, EzyMarshaller marshaller)
        {
            return dataToValue((T)input, marshaller);
        }

        protected abstract T valueToData(object value, EzyUnmarshaller unmarshaller);

        protected abstract object dataToValue(T data, EzyMarshaller marshaller);

        public Type getInType()
        {
            return typeof(T);
        }

        public Type getOutType()
        {
            return typeof(T);
        }
    }

    public abstract class EzyMapConverter<T> : IEzyConverter
    {
        public object read(object input, EzyUnmarshaller unmarshaller)
        {
            EzyObject map = null;
            if (input is IDictionary)
            {
                map = EzyEntityFactory.newObjectBuilder()
                    .appendRawDict((IDictionary)input)
                    .build();
            }
            else
            {
                map = (EzyObject)input;
            }
            return mapToObject(map, unmarshaller);
        }

        public object write(object input, EzyMarshaller marshaller)
        {
            return objectToMap((T)input, marshaller);
        }

        protected abstract T mapToObject(EzyObject map, EzyUnmarshaller unmarshaller);

        protected abstract EzyObject objectToMap(T obj, EzyMarshaller marshaller);

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
            EzyArray array = null;
            if (input is IList)
            {
                array = EzyEntityFactory.newArrayBuilder()
                    .appendRawList((IList)input)
                    .build();
            }
            else
            {
                array = (EzyArray)input;
            }
            return arrayToObject(array, unmarshaller);
        }

        public object write(object input, EzyMarshaller marshaller)
        {
            return objectToArray((T)input, marshaller);
        }

        protected abstract T arrayToObject(EzyArray array, EzyUnmarshaller unmarshaller);

        protected abstract EzyArray objectToArray(T obj, EzyMarshaller marshaller);

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
