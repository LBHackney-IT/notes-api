using NotesApi.V1.Domain;
using System;
using System.ComponentModel.DataAnnotations;

namespace NotesApi.V1.Boundary.Queries
{
    public class CreateNoteRequest
    {
        [Required]
        [MaxLength(500)]
        public string Description { get; set; }

        [Required]
        public TargetType? TargetType { get; set; }

        [Required]
        public Guid? TargetId { get; set; }

        [Required]
        public DateTime? CreatedAt { get; set; }

        public Categorisation Categorisation { get; set; }

        public AuthorDetails Author { get; set; }
    }
}
