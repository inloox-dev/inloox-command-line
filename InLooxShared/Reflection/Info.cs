using System;
using System.Reflection;

namespace InLooxShared.Reflection
{
    public class Info
    {
        public static PropertyInfo GetProperty<T>(string name)
        {
            var t = typeof(T);
            return GetProperty(t, name);
        }

        public static PropertyInfo GetProperty(Type t, string name)
        {
            var prop = t.GetProperty(name);
            return prop;
        }
    }
}
