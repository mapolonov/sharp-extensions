using System;
using System.Globalization;
using System.IO;

namespace Csharp.Extensions
{
    public static class StringExtensions
    {
        public static Stream AsStream(this string source)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            var stream = new MemoryStream();
            var writer = new StreamWriter(stream);

            writer.Write(source);
            writer.Flush();
            stream.Position = 0;

            return stream;
        }

        public static string RemoveSpaces(this string source)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            return source.Replace(" ", string.Empty);
        }

        public static bool CanConvertTo(this string value, Type type)
        {
            if (value == null) throw new ArgumentNullException(nameof(value));
            if (type == null) throw new ArgumentNullException(nameof(type));

            try
            {
                Convert.ChangeType(value, type, CultureInfo.InvariantCulture);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
