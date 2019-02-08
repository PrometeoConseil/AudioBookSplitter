using System.IO;
using System.Linq;

namespace AudioBookSplitterCmd
{
    public static class StringExtensions
    {
        public static string ToFilePathSafeString(this string source, char replaceChar = '_')
        {
            return Path.GetInvalidFileNameChars().Aggregate(source,
                (current, invalidFileNameChar) => current.Replace(invalidFileNameChar, replaceChar)).TrimEnd(' ','.');
        }
    }
}