using AutoFixture;
using FluentAssertions;
using NotesApi.V1.Domain;
using NotesApi.V1.Factories;
using NotesApi.V1.Infrastructure;
using Xunit;

namespace NotesApi.Tests.V1.Factories
{
    public class EntityFactoryTest
    {
        private readonly Fixture _fixture = new Fixture();

        [Fact]
        public void CanMapADatabaseEntityToADomainObject()
        {
            var databaseNote = _fixture.Create<NoteDb>();
            var note = databaseNote.ToDomain();

            databaseNote.Id.Should().Be(note.Id);
            databaseNote.Author.Should().BeEquivalentTo(note.Author);
            databaseNote.Categorisation.Should().BeEquivalentTo(note.Categorisation);
            databaseNote.DateTime.Should().Be(note.DateTime);
            databaseNote.Description.Should().Be(note.Description);
            databaseNote.Tags.Should().Be(note.Tags);
            databaseNote.TargetId.Should().Be(note.TargetId);
            databaseNote.TargetType.Should().Be(note.TargetType);
        }

        [Fact]
        public void CanMapADomainEntityToADatabaseObject()
        {
            var note = _fixture.Create<Note>();
            var databaseNote = note.ToDatabase();

            note.Id.Should().Be(databaseNote.Id);
            note.Author.Should().BeEquivalentTo(databaseNote.Author);
            note.Categorisation.Should().BeEquivalentTo(databaseNote.Categorisation);
            note.DateTime.Should().Be(databaseNote.DateTime);
            note.Description.Should().Be(databaseNote.Description);
            note.Tags.Should().Be(databaseNote.Tags);
            note.TargetId.Should().Be(databaseNote.TargetId);
            note.TargetType.Should().Be(databaseNote.TargetType);
        }
    }
}
