using FluentValidation.TestHelper;
using NotesApi.V1.Boundary.Request.Validation;
using NotesApi.V1.Domain;
using Xunit;

namespace NotesApi.Tests.V1.Boundary.Request.Validation
{
    public class AuthorDetailsValidatorTests
    {
        private readonly AuthorDetailsValidator _sut;

        public AuthorDetailsValidatorTests()
        {
            _sut = new AuthorDetailsValidator();
        }

        private const string ValueWithTags = "sdfsdf<sometag>@abc.com";

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void AuthorDetailsShouldErrorWithNoEmailValue(string value)
        {
            var model = new AuthorDetails() { Email = value };
            var result = _sut.TestValidate(model);
            result.ShouldNotHaveValidationErrorFor(x => x.Email);
        }

        [Theory]
        [InlineData("sdfsdf")]
        [InlineData("sdfsdf<sometag>")]
        public void AuthorDetailsShouldErrorWithInvalidEmail(string invalidEmail)
        {
            var model = new AuthorDetails() { Email = invalidEmail };
            var result = _sut.TestValidate(model);
            result.ShouldHaveValidationErrorFor(x => x.Email)
                .WithErrorCode(ErrorCodes.InvalidEmail);
        }

        [Fact]
        public void AuthorDetailsShouldErrorWithTagsInEmail()
        {
            var model = new AuthorDetails() { Email = ValueWithTags };
            var result = _sut.TestValidate(model);
            result.ShouldHaveValidationErrorFor(x => x.Email)
                .WithErrorCode(ErrorCodes.XssCheckFailure);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void AuthorDetailsShouldErrorWithNoNameValue(string value)
        {
            var model = new AuthorDetails() { FullName = value };
            var result = _sut.TestValidate(model);
            result.ShouldNotHaveValidationErrorFor(x => x.FullName);
        }

        [Fact]
        public void AuthorDetailsShouldErrorWithTagsInName()
        {
            var model = new AuthorDetails() { FullName = ValueWithTags };
            var result = _sut.TestValidate(model);
            result.ShouldHaveValidationErrorFor(x => x.FullName)
                .WithErrorCode(ErrorCodes.XssCheckFailure);
        }
    }
}
