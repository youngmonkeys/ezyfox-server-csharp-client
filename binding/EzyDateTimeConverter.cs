using System;

namespace com.tvd12.ezyfoxserver.client.binding
{
    public class EzyDateTimeConverter : IEzyConverter
    {
        public object read(object input, EzyUnmarshaller unmarshaller)
        {
            return new DateTime(1970, 1, 1).AddMilliseconds((long)input);
        }

        public object write(object input, Ezymarshaller marshaller)
        {
            return ((DateTime)input).Millisecond;
        }

        public Type getInType()
        {
            return typeof(DateTime);
        }

        public Type getOutType()
        {
            return typeof(DateTime);
        }
    }
}
