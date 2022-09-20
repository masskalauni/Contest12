using System;
using System.Collections.Generic;
using System.Linq;

namespace Contest.Web.Extensions
{
    public static class StringExtensions
    {
        public static string ToStringFormatted(this List<string> list, char joiner = ',') => string.Join(joiner, list);
        public static string ToStringFormatted(this string[] list, char joiner = ',') => string.Join(joiner, list);
        public static List<string> ToStringList(this string value, char separator = ',') => value.Split(separator, StringSplitOptions.RemoveEmptyEntries).ToList();
    }
}
