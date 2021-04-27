using System.ComponentModel.DataAnnotations;

namespace NotesApi.V1.Domain.Queries
{
    public class CreateNoteRequest
    {
        [Required]
        [MaxLength(500)]
        public string Description { get; set; }

        public string TargetType { get; set; }

        public string TargetId { get; set; }

        public string DateTime { get; set; }

        public Categorisation Categorisation { get; set; }

        public AuthorDetails Author { get; set; }
    }
}
