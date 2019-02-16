using System;
using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace Csharp.Extensions
{
    public static class XDocumentExtensions
    {
        public static Stream AsStream(this XDocument source)
        {
            if(source == null) throw new ArgumentNullException(nameof(source));

            var stream = new MemoryStream();
            source.Save(stream, SaveOptions.DisableFormatting);
            stream.Position = 0;

            return stream;
        }

        public static XDocument RemoveNamespaces(this XDocument source)
        {
            if(source == null) throw new ArgumentNullException(nameof(source));

            var xDocument = new XDocument(source);
            foreach (var node in xDocument.Descendants())
            {
                node.Attributes().Where(x => x.IsNamespaceDeclaration).Remove();
                node.Name = node.Name.LocalName;
            }

            return xDocument;
        }
    }
}
