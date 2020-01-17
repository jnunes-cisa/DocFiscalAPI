using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;

namespace Common.Extensions
{
    public static class EnumExtension
    {
        public static string Description<T>(this T source)
        {
            if (source != null)
            {
                FieldInfo fi = source.GetType().GetField(source.ToString());
                if (fi != null)
                {
                    DescriptionAttribute[] attributes = (DescriptionAttribute[])fi.GetCustomAttributes(typeof(DescriptionAttribute), false);

                    if (attributes != null && attributes.Length > 0)
                        return attributes[0].Description;
                    else
                        return source.ToString();
                }
            }
            return string.Empty;
        }

        public static T GetEnumFromString<T>(string value)
        {

            if (Enum.IsDefined(typeof(T), value))
            {
                return (T)Enum.Parse(typeof(T), value, true);
            }
            else
            {
                string[] enumNames = Enum.GetNames(typeof(T));

                foreach (string enumName in enumNames)
                {
                    object e = Enum.Parse(typeof(T), enumName);
                    if (value == e.Description())
                    {
                        return (T)e;
                    }
                }
            }

            return (T)Enum.Parse(typeof(T), "0", true);
        }

        public static Dictionary<string, int> GetDictionary<T>()
        {
            Dictionary<string, int> dic = new Dictionary<string, int>();
            string[] enumNames = Enum.GetNames(typeof(T));

            foreach (string enumName in enumNames)
            {
                var e = Enum.Parse(typeof(T), enumName);

                dic.Add(e.Description(), (int)e);

            }

            return dic;
        }

        public static string GetEnumFullName(string tipo)
        {
            return $"Common.Enums.EnumEntities+{tipo}, Common, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null";
        }


    }
}