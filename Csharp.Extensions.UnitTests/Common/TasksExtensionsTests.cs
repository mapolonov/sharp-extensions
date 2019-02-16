using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoFixture;
using FluentAssertions;
using Xunit;

namespace Csharp.Extensions.UnitTests.Common
{
    public class TasksExtensionsTests : TestBase
    {
        [Fact]
        public void TasksExtensions_ThrottleAsync_should_not_accept_null_as_parameter_tasks()
        {
            //Act
            Func<Task> constructor = () => TasksExtensions.ThrottleAsync((IEnumerable<Task>)null, 1);

            //Assert
            constructor.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void TasksExtensions_ThrottleAsync_generic_should_not_accept_null_as_parameter_tasks()
        {
            //Act
            Func<Task> constructor = () => TasksExtensions.ThrottleAsync((IEnumerable<Task<int>>)null, 1);

            //Assert
            constructor.Should().Throw<ArgumentNullException>();
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public void TasksExtensions_ThrottleAsync_should_not_accept_non_negative_value_or_zero_as_parameter_maxConcurrentTasks(int maxConcurrentTasks)
        {
            //Arrange
            var tasks = new Fixture().CreateMany<Task>(10);

            //Act
            Func<Task> constructor = () => TasksExtensions.ThrottleAsync(tasks, maxConcurrentTasks);

            //Assert
            constructor.Should().Throw<ArgumentOutOfRangeException>();
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public void TasksExtensions_ThrottleAsync_generic_should_not_accept_non_negative_value_or_zero_as_parameter_maxConcurrentTasks(int maxConcurrentTasks)
        {
            //Arrange
            var tasks = new Fixture().CreateMany<Task<int>>(10);

            //Act
            Func<Task> constructor = () => TasksExtensions.ThrottleAsync(tasks, maxConcurrentTasks);

            //Assert
            constructor.Should().Throw<ArgumentOutOfRangeException>();
        }
    }
}