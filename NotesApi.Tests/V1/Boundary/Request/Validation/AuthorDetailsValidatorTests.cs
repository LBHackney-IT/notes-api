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
        [InlineData("sdfsdf<sometag>")]
        public void AuthorDetailsShouldErrorWithInvalidEmail(string invalidEmail)
        {
            var model = new AuthorDetails() { Email = invalidEmail };
            var result = _sut.TestValidate(model);
            result.ShouldHaveValidationErrorFor(x => x.Email);
        }

        [Theory]
        [InlineData("sdfsdf<sometag>")]
        public void AuthorDetailsShouldErrorWithInvalidName(string invalidName)
        {
            var model = new AuthorDetails() { FullName = invalidName };
            var result = _sut.TestValidate(model);
            result.ShouldHaveValidationErrorFor(x => x.FullName);
        }
    }
}
