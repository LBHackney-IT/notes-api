using System;
using NotesApi.V2.Domain;

namespace NotesApi.V2.Boundary.Request
{
    public class CreateNoteRequest
    {
        public string Description { get; set; }

        public TargetType? TargetType { get; set; }

        public Guid? TargetId { get; set; }

        public DateTime? CreatedAt { get; set; }

        public Categorisation Categorisation { get; set; }

        public AuthorDetails Author { get; set; }
    }
}
