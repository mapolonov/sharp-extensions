using System;
using System.Collections.Generic;
using Csharp.Extensions.Auth;
using FakeItEasy;
using FluentAssertions;
using Xunit;

namespace Csharp.Extensions.UnitTests.Common
{
    public class TokenGeneratorExtensionsTests
    {
        [Theory]
        [MemberData(nameof(IncompleteMocks))]
        public void TokenGeneratorExtensionsTests_should_throw_if_argument_is_null(BearerOptions options, UserAuthProfile userAuthProfile)
        {
            //Act
            Action action = () => options.GenerateToken(userAuthProfile);

            //Assert
            action.Should().Throw<ArgumentNullException>();
        }

        public static IEnumerable<object[]> IncompleteMocks => new[]
        {
            new object[] { null, A.Fake<UserAuthProfile>() },
            new object[] { A.Fake<BearerOptions>(), null }
        };
    }
}
