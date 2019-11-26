using System;

namespace InLooxShared.Reflection
{
    public static class StringToObject
    {
        public static object Parse(Type propertyType, string value)
        {
            var type = Nullable.GetUnderlyingType(propertyType) ?? propertyType;

            if (type == typeof(double))
                return double.Parse(value);
            if (type == typeof(int))
                return int.Parse(value);
            if (type == typeof(DateTime))
                return DateTime.Parse(value);
            if (type == typeof(string))
                return value;
            if (type == typeof(bool))
                return bool.Parse(value);
            if (type == typeof(Guid))
                return Guid.Parse(value);
            if (type == typeof(DateTimeOffset))
                return DateTimeOffset.Parse(value);
            return null;
        }
    }
}
