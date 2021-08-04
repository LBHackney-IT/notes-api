using FluentValidation.TestHelper;
using NotesApi.V1.Boundary.Request;
using NotesApi.V1.Boundary.Request.Validation;
using NotesApi.V1.Domain;
using System;
using Xunit;

namespace NotesApi.Tests.V1.Boundary.Request.Validation
{

    public class CreateNoteRequestValidatorTests
    {
        private readonly CreateNoteRequestValidator _sut;

        public CreateNoteRequestValidatorTests()
        {
            _sut = new CreateNoteRequestValidator();
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void RequestShouldErrorWithNullOrEmptyDescription(string description)
        {
            var model = new CreateNoteRequest() { Description = description };
            var result = _sut.TestValidate(model);
            result.ShouldHaveValidationErrorFor(x => x.Description)
                  .WithErrorCode("W2");
        }

        [Fact]
        public void RequestShouldErrorWithDescriptionTooLong()
        {
            var msgToRepeat = "This description is to long. ";
            string description = "";
            while (description.Length <= 500)
                description += msgToRepeat;
            var model = new CreateNoteRequest() { Description = description };
            var result = _sut.TestValidate(model);
            result.ShouldHaveValidationErrorFor(x => x.Description)
                  .WithErrorCode("W3");
        }

        [Fact(Skip = "List of special characters still under discussion...")]
        public void RequestShouldErrorWithSpecialCharacters()
        {
            string description = "This description is not ^ fine as it # has special ~ characters.";
            var model = new CreateNoteRequest() { Description = description };
            var result = _sut.TestValidate(model);
            result.ShouldHaveValidationErrorFor(x => x.Description)
                  .WithErrorCode("W8");
        }

        [Fact]
        public void RequestShouldNotErrorWithValidDescription()
        {
            string description = "This description is fine.";
            var model = new CreateNoteRequest() { Description = description };
            var result = _sut.TestValidate(model);
            result.ShouldNotHaveValidationErrorFor(x => x.Description);
        }

        [Theory]
        [InlineData(TargetType.person)]
        [InlineData(TargetType.asset)]
        [InlineData(TargetType.repair)]
        [InlineData(TargetType.tenure)]
        public void RequestShouldNotErrorWithValidTargetType(TargetType tt)
        {
            var model = new CreateNoteRequest() { TargetType = tt };
            var result = _sut.TestValidate(model);
            result.ShouldNotHaveValidationErrorFor(x => x.TargetType);
        }

        [Theory]
        [InlineData(null)]
        [InlineData(10)]
        public void RequestShouldErrorWithInvalidValidTargetType(int? val)
        {
            var model = new CreateNoteRequest() { TargetType = (TargetType?) val };
            var result = _sut.TestValidate(model);
            result.ShouldHaveValidationErrorFor(x => x.TargetType);
        }

        [Fact]
        public void RequestShouldErrorWithNullTargetId()
        {
            var model = new CreateNoteRequest() { TargetId = null };
            var result = _sut.TestValidate(model);
            result.ShouldHaveValidationErrorFor(x => x.TargetId);
        }

        [Fact]
        public void RequestShouldErrorWithEmptyTargetId()
        {
            var model = new CreateNoteRequest() { TargetId = Guid.Empty };
            var result = _sut.TestValidate(model);
            result.ShouldHaveValidationErrorFor(x => x.TargetId);
        }

        [Fact]
        public void RequestShouldErrorWithNullCreatedAt()
        {
            var model = new CreateNoteRequest() { CreatedAt = null };
            var result = _sut.TestValidate(model);
            result.ShouldHaveValidationErrorFor(x => x.CreatedAt);
        }

        [Fact]
        public void RequestShouldErrorWithEmptyCreatedAt()
        {
            var model = new CreateNoteRequest() { CreatedAt = default };
            var result = _sut.TestValidate(model);
            result.ShouldHaveValidationErrorFor(x => x.CreatedAt);
        }

        [Fact]
        public void RequestShouldErrorWithAuthorError()
        {
            var model = new CreateNoteRequest() { Author = new AuthorDetails() { Email = "dflgkj" } };
            var result = _sut.TestValidate(model);
            result.ShouldHaveValidationErrorFor(x => x.Author.Email);
        }
    }
}
