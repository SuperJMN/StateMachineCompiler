using System.Text.RegularExpressions;

namespace Smc.Tests
{
    public static class StringMixin
    {
        public static string ReplaceAll(this string self, string regex, string replacement)
        {
            return Regex.Replace(self, regex, replacement);
        }
    }
}