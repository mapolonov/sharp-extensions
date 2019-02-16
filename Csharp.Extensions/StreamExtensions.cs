using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Linq;

namespace Csharp.Extensions
{
    public static class StreamExtensions
    {
        public static Stream Copy(this Stream source)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            source.Position = 0;

            return source.Read(stream => {
                var destination = new MemoryStream();
                stream.CopyTo(destination);
                destination.Position = 0;

                return destination;
            });
        }

        public static string AsString(this Stream source)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            source.Position = 0;
            return source.Read(stream => {
                var streamReader = new StreamReader(stream);
                return streamReader.ReadToEnd();
            });
        }

        public static XDocument AsXDocument(this Stream source)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            var xDocument = XDocument.Load(source);
            source.Position = 0;

            return xDocument;
        }

        public static void Read(this Stream stream, Action<Stream> readAction)
        {
            if (stream == null) throw new ArgumentNullException(nameof(stream));
            if (readAction == null) throw new ArgumentNullException(nameof(readAction));

            var originalPosition = stream.Position;
            readAction(stream);
            stream.Position = originalPosition;
        }

        public static T Read<T>(this Stream stream, Func<Stream, T> readFunction)
        {
            if (stream == null) throw new ArgumentNullException(nameof(stream));
            if (readFunction == null) throw new ArgumentNullException(nameof(readFunction));

            var originalPosition = stream.Position;
            var result = readFunction(stream);
            stream.Position = originalPosition;

            return result;
        }

        public static IEnumerable<string> GetLines(this Stream source)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            using (var streamReader = new StreamReader(source))
            {
                string line;

                while ((line = streamReader.ReadLine()) != null)
                    yield return line;
            }
        }

        public static Stream AddNodeToRoot(this Stream source, string newNodeName, string nodeValue)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (string.IsNullOrWhiteSpace(newNodeName))
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(newNodeName));
            if (string.IsNullOrWhiteSpace(nodeValue))
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(nodeValue));

            var initialPosition = source.Position;
            source.Position = 0;

            var xDocument = XDocument.Load(source);
            var newElement = new XElement(newNodeName, nodeValue);
            xDocument.Root?.AddFirst(newElement);

            source.Position = initialPosition;
            return xDocument.AsStream();
        }
    }
}