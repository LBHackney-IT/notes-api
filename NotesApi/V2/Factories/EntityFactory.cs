using NotesApi.V2.Domain;
using NotesApi.V2.Infrastructure;

namespace NotesApi.V2.Factories
{
    public static class EntityFactory
    {
        public static Note ToDomain(this NoteDb databaseNote)
        {
            return new Note
            {
                Author = databaseNote.Author,
                Categorisation = databaseNote.Categorisation,
                CreatedAt = databaseNote.CreatedAt,
                Description = databaseNote.Description,
                Id = databaseNote.Id,
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
                CreatedAt = note.CreatedAt,
                Description = note.Description,
                Id = note.Id,
                TargetId = note.TargetId,
                TargetType = note.TargetType
            };
        }
    }
}
