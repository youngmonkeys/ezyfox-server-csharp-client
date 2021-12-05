using System;
using System.Reflection;
using System.Collections.Generic;
using com.tvd12.ezyfoxserver.client.entity;
using com.tvd12.ezyfoxserver.client.factory;

namespace com.tvd12.ezyfoxserver.client.binding
{
    public class EzyReflectionArrayConverter<T> : EzyArrayConverter<T>
    {
        private readonly Type objectType;

        public EzyReflectionArrayConverter()
        {
            objectType = typeof(T);
        }

        protected override T arrayToObject(EzyArray array, EzyUnmarshaller unmarshaller)
        {
            T obj = (T)Activator.CreateInstance(objectType);
            PropertyInfo[] properties = objectType.GetProperties();

            for (int i = 0; i < properties.Length; ++i)
            {
                PropertyInfo targetProperty = properties[i];
                EzyValue anno = targetProperty.GetCustomAttribute<EzyValue>();
                int index = anno != null ? anno.index : i;

                if (index >= array.size())
                {
                    continue;
                }

                Type outType = targetProperty.PropertyType;
                object rawValue = array.getByOutType(index, outType);
                if (rawValue == null)
                {
                    continue;
                }
                object value = unmarshaller.unmarshallByOutType(rawValue, outType);
                if (targetProperty.PropertyType == value.GetType())
                {
                    targetProperty.SetValue(obj, value);
                }
                else
                {
                    MethodInfo parseMethod = targetProperty.PropertyType.GetMethod(
                        "TryParse",
                        BindingFlags.Public | BindingFlags.Static,
                        null,
                        new[] {
                            typeof(string),
                            targetProperty.PropertyType.MakeByRefType()
                        },
                        null
                    );

                    if (parseMethod != null)
                    {
                        object[] parameters = new[] { value, null };
                        bool success = (bool)parseMethod.Invoke(null, parameters);
                        if (success)
                        {
                            targetProperty.SetValue(obj, parameters[1]);
                        }

                    }
                }
            }
            return obj;
        }

        protected override EzyArray objectToArray(T obj, EzyMarshaller marshaller)
        {
            int count = 0;
            SortedDictionary<int, object> valueByIndex = new SortedDictionary<int, object>();
            foreach (PropertyInfo property in objectType.GetProperties())
            {
                EzyValue anno = property.GetCustomAttribute<EzyValue>();
                int index = anno != null ? anno.index : count;
                object rawValue = property.GetValue(obj);
                object value = rawValue != null ? marshaller.marshall<object>(rawValue) : null;
                valueByIndex[index] = value;
                ++count;
            }
            EzyArray array = EzyEntityFactory.newArray();
            for (int i = 0; i < count; ++i)
            {
                object value = null;
                if (valueByIndex.ContainsKey(i))
                {
                    value = valueByIndex[i];
                }
                array.add(value);
            }
            return array;
        }
    }
}
