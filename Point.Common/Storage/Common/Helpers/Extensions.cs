using System.Text.RegularExpressions;

namespace Point.Common.Storage.Common.Helpers
{
    public static class Extensions
    {
        public static string RemoveUTF8ControlCharacters(this string text)
        {
            return Regex.Replace(text, @"[\u0000-\u001F]", string.Empty);
        }
    }
}
