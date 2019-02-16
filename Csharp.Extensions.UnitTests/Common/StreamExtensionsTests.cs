using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using AutoFixture;
using FakeItEasy;
using FluentAssertions;
using Xunit;

namespace Csharp.Extensions.UnitTests.Common
{
    public class StreamExtensionsTests : TestBase
    {
        [Fact]
        public void StreamExtensions_Copy_should_not_accept_null_as_parameter()
        {
            //Act
            Action action = () => StreamExtensions.Copy(null);

            //Assert
            action.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void StreamExtensions_Copy_should_should_return_valid_stream()
        {
            //Arrange
            var testedString = Fixture.Create<string>();
            var testedStream = testedString.AsStream();

            //Act
            var result = testedStream.Copy();

            //Assert
            result.Should().NotBeSameAs(testedStream);
            result.AsString().Should().Be(testedString);
        }

        [Fact]
        public void StreamExtensions_AsString_should_not_accept_null_as_parameter()
        {
            //Act
            Action action = () => StreamExtensions.AsString(null);

            //Assert
            action.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void StreamExtensions_AsString_should_return_valid_string()
        {
            //Arrange
            var testedString = Fixture.Create<string>();
            var testedStream = testedString.AsStream();

            //Act
            var result = testedStream.AsString();

            //Assert
            result.Should().Be(testedString);
        }

        [Fact]
        public void StreamExtensions_AsXDocument_should_not_accept_null_as_parameter()
        {
            //Act
            Action action = () => StreamExtensions.AsXDocument(null);

            //Assert
            action.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void StreamExtensions_AsXDocument_should_return_valid_XDocument()
        {
            //Arrange
            const string expectedXml ="<Root><Child1>data1</Child1><Child2>data2</Child2></Root>";
            var testedStream = expectedXml.AsStream();

            //Act
            var result = testedStream.AsXDocument();

            //Assert
            var resultXml = result.ToString(SaveOptions.DisableFormatting);
            resultXml.Should().Be(expectedXml);
        }

        [Theory]
        [MemberData(nameof(IncompleteMocksForRead))]
        public void StreamExtensions_Read_should_throw_if_argument_is_null(Stream stream, Action<Stream> readAction)
        {
            //Act
            Action action = () => stream.Read(readAction);

            //Assert
            action.Should().Throw<ArgumentNullException>();
        }

        [Theory]
        [MemberData(nameof(IncompleteMocksForReadT))]
        public void StreamExtensions_Read_T_should_throw_if_argument_is_null(Stream stream, Func<Stream, Stream> readFunc)
        {
            //Act
            Action action = () => stream.Read(readFunc);

            //Assert
            action.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void StreamExtensions_GetLines_should_not_accept_null_as_parameter()
        {
            //Act
            Action action = () => StreamExtensions.GetLines(null).ToList();

            //Assert
            action.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void StreamExtensions_GetLine_should_return_valid_ienumerable_of_string()
        {
            //Arrange
            var testedStrings = Fixture.Create<IEnumerable<string>>().ToList();
            var joinedString = string.Join(Environment.NewLine, testedStrings);
            var testedStream = joinedString.AsStream();

            //Act
            var result = testedStream.GetLines();

            //Assert
            result.SequenceEqual(testedStrings).Should().BeTrue();
        }

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData(null)]
        public void StreamExtensions_AddNodeToRoot_should_throw_an_ArgumentException_for_invalid_newNodeName(string newNodeName)
        {
            // Arrange
            const string xmlActual = @"<Product></Product>";
            var xDocStream = XDocument.Parse(xmlActual).AsStream();

            // Act
            Action result = () => xDocStream.AddNodeToRoot(newNodeName, Fixture.Create<string>());

            // Assert
            result.Should().Throw<ArgumentException>();
        }

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData(null)]
        public void StreamExtensions_AddNodeToRoot_should_throw_an_ArgumentException_for_invalid_nodeValue(string nodeValue)
        {
            // Arrange
            const string xmlActual = @"<Product></Product>";
            var xDocStream = XDocument.Parse(xmlActual).AsStream();

            // Act
            Action result = () => xDocStream.AddNodeToRoot(Fixture.Create<string>(), nodeValue);

            // Assert
            result.Should().Throw<ArgumentException>();
        }

        [Fact]
        public void StreamExtensions_AddNodeToRoot_should_throw_ArgumentNullException_when_stream_is_null()
        {
            // Arrange
            Stream xDocStream = null;

            // Act
            Action result = () => xDocStream.AddNodeToRoot(Fixture.Create<string>(), Fixture.Create<string>());

            // Assert
            result.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void StreamExtensions_AddNodeToRoot_should_return_correct_stream()
        {
            // Arrange
            const string xmlExpected = @"<Product><WehkampArticleNumber>1234567</WehkampArticleNumber><RecordReference>9789022558829</RecordReference></Product>";
            const string xmlActual = @"<Product><RecordReference>9789022558829</RecordReference></Product>";
            var xDocStream = XDocument.Parse(xmlActual).AsStream();

            // Act
            var resultStream = xDocStream.AddNodeToRoot("WehkampArticleNumber", "1234567");

            // Assert
            XDocument.Load(resultStream).ToString(SaveOptions.DisableFormatting)
                .Should().BeEquivalentTo(xmlExpected);
        }

        [Fact]
        public void StreamExtensions_AddNodeToRoot_should_not_change_position_in_initial_stream()
        {
            // Arrange
            const string xml = @"<Product></Product>";
            var xDocStream = XDocument.Parse(xml).AsStream();
            xDocStream.Position = 5;
            var initialPosition = xDocStream.Position;

            // Act
            xDocStream.AddNodeToRoot("Tag", Fixture.Create<string>());

            // Assert
            xDocStream.Position.Should().Be(initialPosition);
        }

        [Fact]
        public void StreamExtensions_AddNodeToRoot_should_not_dispose_initial_stream()
        {
            // Arrange
            const string xml = @"<Product></Product>";
            var xDocStream = XDocument.Parse(xml).AsStream();

            // Act
            xDocStream.AddNodeToRoot("Tag", Fixture.Create<string>());

            // Assert
            xDocStream.CanRead.Should().Be(true);
        }

        public static IEnumerable<object[]> IncompleteMocksForRead => new[]
        {
            new object[] { null, A.Fake<Action<Stream>>() },
            new object[] { A.Fake<Stream>(), null }
        };

        public static IEnumerable<object[]> IncompleteMocksForReadT => new[]
        {
            new object[] { null, A.Fake<Func<Stream, Stream>>() },
            new object[] { A.Fake<Stream>(), null }
        };
    }
}
