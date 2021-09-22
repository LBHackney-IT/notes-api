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
                Title = databaseNote.Title,
                Id = databaseNote.Id,
                TargetId = databaseNote.TargetId,
                TargetType = databaseNote.TargetType,
                Highlight = databaseNote.Highlight
            };
        }
    }
}
