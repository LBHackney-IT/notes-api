using System;
using FluentValidation.TestHelper;
using NotesApi.V2.Boundary.Request;
using NotesApi.V2.Boundary.Request.Validation;
using NotesApi.V2.Domain;
using Xunit;

namespace NotesApi.Tests.V2.Boundary.Request.Validation
{
    public class CreateNoteRequestValidatorTests
    {
        private readonly CreateNoteRequestValidator _sut;

        public CreateNoteRequestValidatorTests()
        {
            _sut = new CreateNoteRequestValidator();
        }

        private const string StringWithTags = "Some string with <tag> in it.";

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void RequestShouldErrorWithNullOrEmptyDescription(string description)
        {
            var model = new CreateNoteRequest() { Description = description };
            var result = _sut.TestValidate(model);
            result.ShouldHaveValidationErrorFor(x => x.Description)
                  .WithErrorCode(ErrorCodes.DescriptionMandatory);
        }

        [Fact]
        public void RequestShouldErrorWithTagsInDescription()
        {
            var model = new CreateNoteRequest() { Description = StringWithTags };
            var result = _sut.TestValidate(model);
            result.ShouldHaveValidationErrorFor(x => x.Description)
                  .WithErrorCode(ErrorCodes.XssCheckFailure);
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
                  .WithErrorCode(ErrorCodes.DescriptionTooLong);
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
        public void RequestShouldErrorWithNoAuthor()
        {
            var model = new CreateNoteRequest() { Author = null };
            var result = _sut.TestValidate(model);
            result.ShouldHaveValidationErrorFor(x => x.Author);
        }
    }
}
