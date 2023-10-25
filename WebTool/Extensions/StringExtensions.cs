using System;

namespace WebTool
{
    internal static class StringExtensions
    {
        public static bool IgEquals(this string str1, string str2)
        {
            return string.Equals(str1, str2, StringComparison.InvariantCultureIgnoreCase);
        }

        public static bool IsDefined(this string str)
        {
            return !string.IsNullOrEmpty(str)
                && !string.IsNullOrWhiteSpace(str);
        }

        public static string NullIfUndefined(this string str)
        {
            return str.IsDefined() ? str : null;
        }
    }
}