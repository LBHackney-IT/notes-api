using NotesApi.V1.Domain;

namespace NotesApi.V1.Boundary
{
    public class CreateNoteRequest
    {
        public string Description { get; set; }

        public string TargetType { get; set; }

        public string TargetId { get; set; }

        public string DateTime { get; set; }

        public Categorisation Categorisation { get; set; }

        public AuthorDetails Author { get; set; }
    }
}
