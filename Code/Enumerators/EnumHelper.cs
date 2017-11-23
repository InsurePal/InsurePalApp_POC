using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SupportFriends.Code.Enumerators
{
    public static class EnumHelper
    {
        public static T ToEnum<T>(this int enumInt)
        {
            if (Enum.IsDefined(typeof(T), enumInt))
                return (T)Enum.Parse(typeof(T), enumInt.ToString(), true);
            else
                return (T)Enum.Parse(typeof(T), "-1".ToString(), true);
        }
        public static T ToEnum<T>(this string enumString)
        {
            if (Enum.IsDefined(typeof(T), Convert.ToInt32(enumString)))
                return (T)Enum.Parse(typeof(T), enumString, true);
            else
                return (T)Enum.Parse(typeof(T), "-1".ToString(), true);
        }
    }
}