using System;

namespace InLooxShared.Basics
{
    public static class EnumParser
    {
        public static TEnum ParseFuzzy<TEnum>(string val) where TEnum : struct
        {
            var type = typeof(TEnum);
            var values = Enum.GetValues(type);

            foreach (var item in values)
            {
                if (string.Equals(item.ToString(), val, StringComparison.InvariantCultureIgnoreCase))
                    return (TEnum)item;
            }

            throw new ArgumentOutOfRangeException($"{val} is not in {type.Name}");
        }
    }
}
