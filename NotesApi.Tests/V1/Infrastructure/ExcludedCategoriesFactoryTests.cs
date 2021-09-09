using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using NotesApi.V1.Infrastructure;
using Xunit;

namespace NotesApi.Tests.V1.Infrastructure
{
    public class ExcludedCategoriesFactoryTests
    {
        private ExcludedCategoriesFactory _sut;

        public ExcludedCategoriesFactoryTests()
        {
            _sut = new ExcludedCategoriesFactory();
        }

        [Fact]
        public void GivenALotOfParametersWhenCreatingListParameterKeysThenKeysShouldAlwaysBeUniqueIrrespectiveOfValues()
        {
            // Arrange
            var listOfParameters = new List<string>();
            for (int i = 0; i < 1000; i++)
            {
                listOfParameters.Add("Value");
            }

            // Act
            var result = _sut.Create(listOfParameters);

            //Assert
            result.Select(x => x.CategoryValueKey).Distinct().Count().Should().Be(result.Count);
            result.Select(x => x.CategoryKey).Distinct().Count().Should().Be(result.Count);
        }
    }
}
