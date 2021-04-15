using System;

namespace NotesApi.V1.Domain
{
    public class Note
    {
        public Guid Id { get; set; }
        public string Description { get; set; }
        public TargetType TargetType { get; set; }
        public Guid TargetId { get; set; }
        public DateTime DateTime { get; set; }
        public Categorisation Categorisation { get; set; }
        public AuthorDetails Author { get; set; }
    }
}
