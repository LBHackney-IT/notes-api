using System;
using NotesApi.V2.Domain;

namespace NotesApi.V2.Boundary.Response
{
    public class NoteResponseObject
    {
        public Guid Id { get; set; }

        public string Description { get; set; }

        public TargetType TargetType { get; set; }

        public Guid TargetId { get; set; }

        public DateTime CreatedAt { get; set; }

        public Categorisation Categorisation { get; set; }

        public AuthorDetails Author { get; set; }

        public static NoteResponseObject Create(Note note)
        {
            if (note is null) throw new ArgumentNullException(nameof(note));

            return new NoteResponseObject
            {
                Id = note.Id,
                Description = note.Description,
                TargetType = note.TargetType,
                TargetId = note.TargetId,
                CreatedAt = note.CreatedAt,
                Categorisation = new Categorisation
                {
                    Description = note.Categorisation?.Description,
                    Category = note.Categorisation?.Category,
                    SubCategory = note.Categorisation?.SubCategory
                },
                Author = new AuthorDetails
                {
                    Email = note.Author?.Email,
                    FullName = note.Author?.FullName
                }
            };
        }
    }
}
