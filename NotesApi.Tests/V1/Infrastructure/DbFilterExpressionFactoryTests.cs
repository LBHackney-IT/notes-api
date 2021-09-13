using System.Collections.Generic;
using FluentAssertions;
using NotesApi.V1.Gateways;
using NotesApi.V1.Infrastructure;
using Xunit;

namespace NotesApi.Tests.V1.Infrastructure
{
    public class DbFilterExpressionFactoryTests
    {
        private DbFilterExpressionFactory _sut;

        public DbFilterExpressionFactoryTests()
        {
            _sut = new DbFilterExpressionFactory();
        }

        [Fact]
        public void GivenACollectionOfCategoriesWhenCreatingFilterExpressionShouldCreateItCorrectly()
        {
            // Arrange
            var categoriesList = new List<ExcludedCategory>
            {
                new ExcludedCategory {CategoryValueKey = "1"},
                new ExcludedCategory {CategoryValueKey = "2"}
            };

            // Act
            var result = _sut.Create(categoriesList);

            // Assert
            result.Should().Be("#categorisation.#category <> 1 and #categorisation.#category <> 2");
        }
    }
}
