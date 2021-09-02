using System.Collections.Generic;
using AutoFixture;
using FluentAssertions;
using NotesApi.V2.Domain;
using NotesApi.V2.Factories;
using Xunit;

namespace NotesApi.Tests.V2.Factories
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
            note.CreatedAt.Should().Be(responseNote.CreatedAt);
            note.Description.Should().Be(responseNote.Description);
            note.TargetId.Should().Be(responseNote.TargetId);
            note.TargetType.Should().Be(responseNote.TargetType);
        }

        [Fact]
        public void CanMapDomainNotesToANoteResponsesList()
        {
            var notes = _fixture.CreateMany<Note>(10);
            var responseNotes = notes.ToResponse();

            responseNotes.Should().BeEquivalentTo(notes);
        }

        [Fact]
        public void CanMapNullDomainNotesToAnEmptyNoteResponsesList()
        {
            List<Note> notes = null;
            var responseNotes = notes.ToResponse();

            responseNotes.Should().BeEmpty();
        }
    }
}
