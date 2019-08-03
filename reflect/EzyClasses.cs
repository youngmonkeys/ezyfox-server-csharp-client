using System;
using System.Text;

namespace com.tvd12.ezyfoxserver.client.reflect
{
    public sealed class EzyClasses
    {
        private static readonly char DOT = '.';

        private EzyClasses()
        {
        }

        public static String getClassName(Type clazz, int includePackages) {
            String fullName = clazz.FullName;
            StringBuilder builder = new StringBuilder();
            int passedPackages = 0;
            for (int i = fullName.Length - 1; i >= 0; --i) {
                char ch = fullName[i];
                if(ch == DOT) {
                    if ((++passedPackages) > includePackages)
                        break;
                }
                builder.Insert(0, ch);
            }
            return builder.ToString();
        }
    }
}
