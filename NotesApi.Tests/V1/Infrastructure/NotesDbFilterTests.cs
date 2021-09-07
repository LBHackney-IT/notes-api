using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using NotesApi.V1.Domain;
using NotesApi.V1.Infrastructure;
using Xunit;

namespace NotesApi.Tests.V1.Infrastructure
{
    public class NotesDbFilterTests
    {
        private NotesDbFilter _sut;

        public NotesDbFilterTests()
        {
            _sut = new NotesDbFilter();
        }

        [Fact]
        public void GivenACollectionOfNotesWhenFilteringThenASBCategoriesAreFilteredOut()
        {
            // Arrange
            var notes = new List<Note>
            {
                new Note {Categorisation = new Categorisation {Category = "ASB"}},
                new Note {Categorisation = new Categorisation {Category = "NotASB"}}
            };

            // Act
            var result = _sut.Filter(notes);

            // Assert
            result.Count.Should().Be(1);
            result.First().Categorisation.Category.Should().Be("NotASB");
        }
    }
}
