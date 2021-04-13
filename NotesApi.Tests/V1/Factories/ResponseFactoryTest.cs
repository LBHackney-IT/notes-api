using AutoFixture;
using FluentAssertions;
using NotesApi.V1.Domain;
using NotesApi.V1.Factories;
using Xunit;

namespace NotesApi.Tests.V1.Factories
{
    public class ResponseFactoryTest
    {
        private readonly Fixture _fixture = new Fixture();

        [Fact]
        public void CanMapADomainNoteToANoteResponse()
        {
            var note = _fixture.Create<Note>();
            var responseNote = note.ToResponse();

            note.Id.Should().Be(responseNote.Id);
            note.Author.Should().BeEquivalentTo(responseNote.Author);
            note.Categorisation.Should().BeEquivalentTo(responseNote.Categorisation);
            note.DateTime.Should().Be(responseNote.DateTime);
            note.Description.Should().Be(responseNote.Description);
            note.Tags.Should().Be(responseNote.Tags);
            note.TargetId.Should().Be(responseNote.TargetId);
            note.TargetType.Should().Be(responseNote.TargetType);
        }
    }
}
