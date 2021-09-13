using NotesApi.V1.Domain;
using System;

namespace NotesApi.Tests.V1.Helper
{
    public static class Constants
    {
        public static Guid ID { get; } = Guid.NewGuid();
        public static Guid TARGETID { get; } = Guid.NewGuid();
        public const TargetType TARGETTYPE = TargetType.person;
        public const string DESCRIPTION = "This is some note description";
        public static DateTime DATETIME { get; } = DateTime.UtcNow.AddDays(-20);
        public const string PREFSURNAME = "Roberts";
        public const string FIRSTNAME = "Robert";

        public const string CATEGORISATIONCATEGORY = "Categroy";
        public const string CATEGORISATIONSUBCATEGORY = "Some sub categroy";
        public const string CATEGORISATIONDESCRIPTION = "Some category description";

        public const string AUTHORFULLNAME = "Bob Roberts";
        public const string AUTHOREMAIL = "bob.roberts@ccc.com";

        public static Note ConstructNoteFromConstants()
        {
            var note = new Note();
            note.Id = Constants.ID;
            note.TargetId = Constants.TARGETID;
            note.TargetType = Constants.TARGETTYPE;
            note.Description = Constants.DESCRIPTION;
            note.CreatedAt = Constants.DATETIME;
            note.Categorisation = new Categorisation
            {
                Category = CATEGORISATIONCATEGORY,
                SubCategory = CATEGORISATIONSUBCATEGORY,
                Description = CATEGORISATIONDESCRIPTION
            };
            note.Author = new AuthorDetails
            {
                FullName = AUTHORFULLNAME,
                Email = AUTHOREMAIL
            };
            return note;
        }
    }
}
