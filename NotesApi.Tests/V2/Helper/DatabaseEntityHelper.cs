using AutoFixture;
using NotesApi.V2.Domain;
using NotesApi.V2.Factories;
using NotesApi.V2.Infrastructure;

namespace NotesApi.Tests.V2.Helper
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
