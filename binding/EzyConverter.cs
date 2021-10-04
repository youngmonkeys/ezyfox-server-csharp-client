using com.tvd12.ezyfoxserver.client.entity;

namespace com.tvd12.ezyfoxserver.client.binding
{
    public interface EzyReader
    {
        object read(object input); 
    }

    public interface EzyWriter
    {
        object write(object input);
    }

    public abstract class EzyMapToObject<T> : EzyReader
    {
        public object read(object input)
        {
            return mapToObject((EzyObject)input);
        }

        protected abstract T mapToObject(EzyObject map);
    }

    public abstract class EzyArrayToObject<T> : EzyReader
    {
        public object read(object input)
        {
            return arrayToObject((EzyArray)input);
        }

        protected abstract T arrayToObject(EzyArray array);
    }

    public abstract class EzyobjectToMap<T> : EzyWriter
    {
        public object write(object input)
        {
            return objectToMap((T)input);
        }

        protected abstract EzyObject objectToMap(T obj);
    }

    public abstract class EzyobjectToArray<T> : EzyWriter
    {
        public object write(object input)
        {
            return objectToArray((T)input);
        }

        protected abstract EzyArray objectToArray(T obj);
    }

    public interface EzyConverter: EzyReader, EzyWriter
    {
    }

    public abstract class EzyMapConverter<T> : EzyReader, EzyWriter
    {
        public object read(object input)
        {
            return mapToObject((EzyObject)input);
        }

        public object write(object input)
        {
            return objectToMap((T)input);
        }

        protected abstract T mapToObject(EzyObject map);

        protected abstract EzyObject objectToMap(T obj);
    }

    public abstract class EzyArrayConverter<T> : EzyReader, EzyWriter
    {
        public object read(object input)
        {
            return arrayToObject((EzyArray)input);
        }

        public object write(object input)
        {
            return objectToArray((T)input);
        }

        protected abstract T arrayToObject(EzyArray array);

        protected abstract EzyArray objectToArray(T obj);
    }
}
