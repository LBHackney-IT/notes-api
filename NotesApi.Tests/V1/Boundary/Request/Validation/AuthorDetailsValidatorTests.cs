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

        [Theory]
        [InlineData("sdfsdf")]
        public void AuthorDetailsShouldErrorWithInvalidEmail(string invalidEmail)
        {
            var model = new AuthorDetails() { Email = invalidEmail };
            var result = _sut.TestValidate(model);
            result.ShouldHaveValidationErrorFor(x => x.Email);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("sdfsdf")]
        public void AuthorDetailsShouldErrorWithInvalidEmailWithValidId(string invalidEmail)
        {
            var model = new AuthorDetails() { Id = "some-id", Email = invalidEmail };
            var result = _sut.TestValidate(model);
            result.ShouldHaveValidationErrorFor(x => x.Email);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void AuthorDetailsShouldErrorWithInvalidNameWithValidId(string invalidName)
        {
            var model = new AuthorDetails() { Id = "some-id", FullName = invalidName };
            var result = _sut.TestValidate(model);
            result.ShouldHaveValidationErrorFor(x => x.FullName);
        }
    }
}
