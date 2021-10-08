using System;

namespace com.tvd12.ezyfoxserver.client.binding
{
    [AttributeUsage(AttributeTargets.Property)]
    public class EzyValue : System.Attribute
    {
        public readonly string name;
        public readonly int index;

        public EzyValue(string name): this(name, 0)
        {
        }

        public EzyValue(int index) : this("", index)
        {
        }

        public EzyValue(string name, int index)
        {
            this.name = name;
            this.index = index;
        }
    }
}
