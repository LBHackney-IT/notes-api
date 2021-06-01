using NotesApi.V1.Domain;
using System;
using System.ComponentModel.DataAnnotations;

namespace NotesApi.V1.Boundary.Request
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
