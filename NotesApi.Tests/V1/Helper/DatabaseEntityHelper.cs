using AutoFixture;
using NotesApi.V1.Domain;
using NotesApi.V1.Factories;
using NotesApi.V1.Infrastructure;

namespace NotesApi.Tests.V1.Helper
{
    public static class DatabaseEntityHelper
    {
        public static NoteDb CreateDatabaseEntity()
        {
            var note = new Fixture().Create<Note>();
            return note.ToDatabase();
        }
    }
}
