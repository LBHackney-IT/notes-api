using FluentAssertions;
using NotesApi.Tests.V1.Helper;
using NotesApi.V1.Domain;
using Xunit;

namespace NotesApi.Tests.V1.Domain
{
    public class NoteTests
    {
        [Fact]
        public void NoteHasPropertiesSet()
        {
            Note note = Constants.ConstructNoteFromConstants();

            note.Id.Should().Be(Constants.ID);
            note.TargetId.Should().Be(Constants.TARGETID);
            note.TargetType.Should().Be(Constants.TARGETTYPE);
            note.Description.Should().Be(Constants.DESCRIPTION);
            note.DateTime.Should().Be(Constants.DATETIME);
            note.Categorisation.Category.Should().Be(Constants.CATEGORISATIONCATEGORY);
            note.Categorisation.SubCategory.Should().Be(Constants.CATEGORISATIONSUBCATEGORY);
            note.Categorisation.Description.Should().Be(Constants.CATEGORISATIONDESCRIPTION);
            note.Author.Email.Should().Be(Constants.AUTHOREMAIL);
            note.Author.FullName.Should().Be(Constants.AUTHORFULLNAME);
            note.Author.Id.Should().Be(Constants.AUTHORID);
        }
    }
}
