using System;
using System.Collections.Generic;
using AutoFixture;
using FluentAssertions;
using Xunit;

namespace Csharp.Extensions.UnitTests.Common
{
    public class StringExtensionsTests
    {
        [Fact]
        public void StringExtensions_AsStream_should_not_accept_null_as_parameter()
        {
            //Act
            Action action = () => StringExtensions.AsStream(null);

            //Assert
            action.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void StringExtensions_AsStream_should_return_valid_stream()
        {
            //Arrange
            var testedString = new Fixture().Create<string>();

            //Act
            var result = testedString.AsStream();

            //Assert
            result.AsString().Should().Be(testedString);
        }

        [Fact]
        public void StringExtensions_RemoveSpaces_should_not_accept_null_as_parameter()
        {
            //Act
            Action action = () => StringExtensions.RemoveSpaces(null);

            //Assert
            action.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void StringExtensions_RemoveSpaces_should_remove_spaces()
        {
            //Arrange
            const string testedString = " a b c ";

            //Act
            var result = testedString.RemoveSpaces();

            //Assert
            result.Should().Be("abc");
        }

        [Theory]
        [MemberData(nameof(IncompleteMocksCanConvertTo))]
        public void StringExtensions_CanConvertTo_should_throw_if_argument_is_null(string value, Type type)
        {
            //Act
            Action action = () => value.CanConvertTo(type);

            //Assert
            action.Should().Throw<ArgumentNullException>();
        }

        [Theory]
        [MemberData(nameof(ValidIntegerValues))]
        public void StringExtensions_CanConvertTo_should_return_true_for_valid_integer_values(string value)
        {
            //Arrange
            var type = typeof(int);

            //Act
            var result = value.CanConvertTo(type);

            //Assert
            result.Should().BeTrue();
        }

        [Theory]
        [MemberData(nameof(InvalidIntegerValues))]
        public void StringExtensions_CanConvertTo_should_return_false_for_invalid_integer_values(string value)
        {
            //Arrange
            var type = typeof(int);

            //Act
            var result = value.CanConvertTo(type);

            //Assert
            result.Should().BeFalse();
        }

        [Fact]
        public void StringExtensions_CanConvertTo_should_return_false_for_invalid_integer_value_with_unvisible_chars()
        {
            //Arrange
            var badChar = char.ConvertFromUtf32(8234); //U+202A	LEFT-TO-RIGHT EMBEDDING :: UnicodeCategory.Format group
            var badIntString = string.Concat("123", badChar);
            var type = typeof(int);

            //Act
            var result = badIntString.CanConvertTo(type);

            //Assert
            result.Should().BeFalse();
        }

        public static IEnumerable<object[]> IncompleteMocksCanConvertTo => new[]
        {
            new object[] { null, new Fixture().Create<Type>() },
            new object[] { new Fixture().Create<string>(), null }
        };

        public static IEnumerable<object[]> ValidIntegerValues => new[]
        {
            new object[] { int.MinValue.ToString() },
            new object[] { int.MaxValue.ToString() },
            new object[] { "0" },
            new object[] { "00000000000001" }
        };

        public static IEnumerable<object[]> InvalidIntegerValues => new[]
        {
            new object[] { "" },
            new object[] { " " },
            new object[] { "_" },
            new object[] { "a" },
            new object[] { "a1" },
            new object[] { "." },
            new object[] { long.MaxValue.ToString() },
        };
    }
}
