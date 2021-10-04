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

        protected override T mapToObject(EzyObject map)
        {
            T obj = (T)Activator.CreateInstance(objectType);

            foreach (object rawKey in map.keys())
            {
                var keyString = rawKey.ToString();

                var targetProperty = objectType.GetProperty(keyString);
                if(targetProperty == null)
                {
                    var key = char.ToUpper(keyString[0]) + keyString.Substring(1);
                    targetProperty = objectType.GetProperty(key);
                }

                var value = map.get(rawKey, targetProperty.PropertyType);
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

        protected override EzyObject objectToMap(T obj)
        {
            EzyObject map = EzyEntityFactory.newObject();
            foreach (PropertyInfo property in objectType.GetProperties())
            {
                string key = property.Name.Length <= 1
                    ? char.ToLower(property.Name[0]).ToString()
                    : char.ToLower(property.Name[0]) + property.Name.Substring(1);
                object value = property.GetValue(obj);
                map.put(key, value);
            }
            return map;
        }
    }
}
