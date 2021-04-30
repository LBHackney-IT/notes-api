using System;
using System.ComponentModel.DataAnnotations;

namespace NotesApi.V1.Domain.Queries
{
    public class CreateNoteRequest
    {
        [Required]
        [MaxLength(500)]
        public string Description { get; set; }

        public TargetType TargetType { get; set; }

        public Guid TargetId { get; set; }

        public DateTime CreatedAt { get; set; }

        public Categorisation Categorisation { get; set; }

        public AuthorDetails Author { get; set; }
    }
}
