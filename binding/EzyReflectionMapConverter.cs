using System;
using System.Reflection;
using com.tvd12.ezyfoxserver.client.entity;
using com.tvd12.ezyfoxserver.client.factory;

namespace com.tvd12.ezyfoxserver.client.binding
{
    public class EzyReflectionMapConverter<T> : EzyMapConverter<T>
    {
        private readonly Type objectType;

        public EzyReflectionMapConverter()
        {
            objectType = typeof(T);
        }

        protected override T mapToObject(EzyObject map, EzyUnmarshaller unmarshaller)
        {
            PropertyInfo[] properties = objectType.GetProperties();

            T obj = (T)Activator.CreateInstance(objectType);

            foreach (PropertyInfo property in properties)
            {
                Type outType = property.PropertyType;

                object rawValue = null;
                EzyValue anno = property.GetCustomAttribute<EzyValue>();
                if (anno != null)
                {
                    rawValue = map.getByOutType(anno.name, outType);
                }
                else
                {
                    rawValue = map.getByOutType(property.Name, outType);
                    if (rawValue == null)
                    {
                        string keyString = char.ToUpper(property.Name[0]).ToString();
                        if (property.Name.Length > 1)
                        {
                            keyString += property.Name.Substring(1);
                        }
                        rawValue = map.getByOutType(keyString, outType);
                    }
                }
                if (rawValue == null)
                {
                    continue;
                }

                object value = unmarshaller.unmarshallByOutType(rawValue, outType);
                if (outType == value.GetType())
                {
                    property.SetValue(obj, value);
                }
                else
                {
                    MethodInfo parseMethod = property.PropertyType.GetMethod(
                        "TryParse",
                        BindingFlags.Public | BindingFlags.Static,
                        null,
                        new[] {
                            typeof(string),
                            property.PropertyType.MakeByRefType()
                        },
                        null
                    );

                    if (parseMethod != null)
                    {
                        object[] parameters = new[] { value, null };
                        bool success = (bool)parseMethod.Invoke(null, parameters);
                        if (success)
                        {
                            property.SetValue(obj, parameters[1]);
                        }

                    }
                }
            }
            return obj;
        }

        protected override EzyObject objectToMap(T obj, Ezymarshaller marshaller)
        {
            EzyObject map = EzyEntityFactory.newObject();
            foreach (PropertyInfo property in objectType.GetProperties())
            {
                string key = null;
                EzyValue anno = property.GetCustomAttribute<EzyValue>();
                if (anno != null)
                {
                    key = anno.name;
                }
                else
                {
                    key = property.Name.Length <= 1
                        ? char.ToLower(property.Name[0]).ToString()
                        : char.ToLower(property.Name[0]) + property.Name.Substring(1);
                }
                object rawValue = property.GetValue(obj);
                object value = rawValue != null ? marshaller.marshall<object>(rawValue) : null;
                map.put(key, value);
            }
            return map;
        }
    }
}
