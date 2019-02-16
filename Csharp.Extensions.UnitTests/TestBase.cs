using AutoFixture;

namespace Csharp.Extensions.UnitTests
{
    public class TestBase
    {
        protected static IFixture Fixture { get; }

        static TestBase()
        {
            Fixture = new Fixture();
        }
    }
}
