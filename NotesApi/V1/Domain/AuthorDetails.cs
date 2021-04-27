using System.ComponentModel.DataAnnotations;

namespace NotesApi.V1.Domain
{
    public class AuthorDetails
    {
        public string Id { get; set; }

        public string FullName { get; set; }

        [EmailAddress]
        public string Email { get; set; }
    }
}
