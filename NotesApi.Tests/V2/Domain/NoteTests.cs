using FluentAssertions;
using NotesApi.Tests.V2.Helper;
using NotesApi.V2.Domain;
using Xunit;

namespace NotesApi.Tests.V2.Domain
{
    public class NoteTests
    {
        [Fact]
        public void NoteHasPropertiesSet()
        {
            Note note = Constants.ConstructNoteFromConstants();

            note.Id.Should().Be(Constants.ID);
            note.Title.Should().Be(Constants.TITLE);
            note.TargetId.Should().Be(Constants.TARGETID);
            note.TargetType.Should().Be(Constants.TARGETTYPE);
            note.Description.Should().Be(Constants.DESCRIPTION);
            note.CreatedAt.Should().Be(Constants.DATETIME);
            note.Highlight.Should().Be(Constants.HIGHLIGHT);
            note.Categorisation.Category.Should().Be(Constants.CATEGORISATIONCATEGORY);
            note.Categorisation.SubCategory.Should().Be(Constants.CATEGORISATIONSUBCATEGORY);
            note.Categorisation.Description.Should().Be(Constants.CATEGORISATIONDESCRIPTION);
            note.Author.Email.Should().Be(Constants.AUTHOREMAIL);
            note.Author.FullName.Should().Be(Constants.AUTHORFULLNAME);
        }
    }
}
