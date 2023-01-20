using System;
using System.Globalization;

namespace com.tvd12.ezyfoxserver.client.binding
{
    public class EzyDateTimeConverter : EzyDataConverter<DateTime>
    {
        protected override DateTime valueToData(
            object value,
            EzyUnmarshaller unmarshaller)
        {
            if (value is Int64)
            {
                return new DateTime(1970, 1, 1).AddMilliseconds((long)value);
            }
            if (value is Int32)
            {
                return new DateTime(1970, 1, 1).AddMilliseconds((Int32)value);
            }
            if (value is string)
            {
                try
                {
                    return DateTime.ParseExact(
                        (string)value,
                        "yyyy-MM-dd'T'HH:mm:ss:fff",
                        CultureInfo.InvariantCulture);
                }
                catch (Exception)
                {
                    return DateTime.ParseExact(
                        (string)value,
                        "yyyy-MM-dd'T'HH:mm:ss.fff",
                        CultureInfo.InvariantCulture);
                }
            }
            return (DateTime)value;
        }

        protected override object dataToValue(
            DateTime data,
            EzyMarshaller marshaller)
        {
            return data.Millisecond;
        }
    }
}
