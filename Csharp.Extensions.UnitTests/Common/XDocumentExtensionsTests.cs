using System;
using System.Xml.Linq;
using FluentAssertions;
using Xunit;

namespace Csharp.Extensions.UnitTests.Common
{
    public class XDocumentExtensionsTests : TestBase
    {
        [Fact]
        public void XDocumentExtensions_AsStream_should_not_accept_null_as_parameter()
        {
            //Act
            Action action = () => XDocumentExtensions.AsStream(null);

            //Assert
            action.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void XDocumentExtensions_AsStream_should_return_valid_stream()
        {
            //Arrange
            const string xml = "<?xml version=\"1.0\" encoding=\"utf-8\"?><Root></Root>";
            var xDocument = XDocument.Parse(xml);
            
            //Act
            var result = xDocument.AsStream();

            //Assert
            result.Should().NotBeNull();
            result.Position.Should().Be(0);
            result.AsString().Should().Be(xml);
        }

        [Fact]
        public void XDocumentExtensions_AsStream_should_return_valid_stream_with_added_declaration()
        {
            //Arrange
            const string xml = "<Root></Root>";
            var xDocument = XDocument.Parse(xml);

            const string expectedXml = "<?xml version=\"1.0\" encoding=\"utf-8\"?><Root></Root>";

            //Act
            var result = xDocument.AsStream();

            //Assert
            result.Should().NotBeNull();
            result.Position.Should().Be(0);
            result.AsString().Should().Be(expectedXml);
        }

        [Fact]
        public void XDocumentExtensions_RemoveNamespaces_should_not_accept_null_as_parameter()
        {
            //Act
            Action action = () => XDocumentExtensions.RemoveNamespaces(null);

            //Assert
            action.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void XDocumentExtensions_RemoveNamespaces_should_return_valid_xDocument()
        {
            //Arrange
            const string xml = "<?xml version=\"1.0\" encoding=\"utf-8\"?><Root xmlns=\"http://testNamespace\"></Root>";
            var xDocument = XDocument.Parse(xml);

            const string expectedXml = "<Root></Root>";

            //Act
            var result = xDocument.RemoveNamespaces();

            //Assert
            result.Should().NotBeNull();
            result.ToString().Should().Be(expectedXml);
        }
    }
}
