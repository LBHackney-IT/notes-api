using System;
using AutoFixture;
using FluentAssertions;
using NotesApi.V2.Boundary.Response;
using NotesApi.V2.Domain;
using Xunit;

namespace NotesApi.Tests.V2.Boundary.Response
{
    public class NoteResponseObjectTests
    {
        private readonly Fixture _fixture = new Fixture();

        [Fact]
        public void CreateTestNullNoteThrows()
        {
            Action action = () => NoteResponseObject.Create(null);
            action.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void CreateTestWithNoteReturnsResponseObject()
        {
            var note = _fixture.Create<Note>();
            var result = NoteResponseObject.Create(note);
            result.Should().BeEquivalentTo(note);
        }

        [Fact]
        public void CreateTestWithNoteWIthNullCategoryReturnsResponseObject()
        {
            var note = _fixture.Create<Note>();
            note.Categorisation = null;
            var result = NoteResponseObject.Create(note);
            result.Should().BeEquivalentTo(note, (opt) => opt.Excluding(x => x.Categorisation));
            result.Categorisation.Should().BeEquivalentTo(new Categorisation());
        }

        [Fact]
        public void CreateTestWithNoteWIthNullAuthorReturnsResponseObject()
        {
            var note = _fixture.Create<Note>();
            note.Author = null;
            var result = NoteResponseObject.Create(note);
            result.Should().BeEquivalentTo(note, (opt) => opt.Excluding(x => x.Author));
            result.Author.Should().BeEquivalentTo(new AuthorDetails());
        }
    }
}
