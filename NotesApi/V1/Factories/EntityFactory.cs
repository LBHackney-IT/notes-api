using NotesApi.V1.Domain;
using NotesApi.V1.Infrastructure;

namespace NotesApi.V1.Factories
{
    public static class EntityFactory
    {
        public static Note ToDomain(this NoteDb databaseNote)
        {
            return new Note
            {
                Author = databaseNote.Author,
                Categorisation = databaseNote.Categorisation,
                DateTime = databaseNote.DateTime,
                Description = databaseNote.Description,
                Id = databaseNote.Id,
                Tags = databaseNote.Tags,
                TargetId = databaseNote.TargetId,
                TargetType = databaseNote.TargetType
            };
        }

        public static NoteDb ToDatabase(this Note note)
        {
            return new NoteDb
            {
                Author = note.Author,
                Categorisation = note.Categorisation,
                DateTime = note.DateTime,
                Description = note.Description,
                Id = note.Id,
                Tags = note.Tags,
                TargetId = note.TargetId,
                TargetType = note.TargetType
            };
        }
    }
}
