using FluentValidation.TestHelper;
using NotesApi.V2.Boundary.Request.Validation;
using NotesApi.V2.Domain;
using Xunit;

namespace NotesApi.Tests.V2.Boundary.Request.Validation
{
    public class CategorisationValidatorTests
    {
        private readonly CategorisationValidator _sut;

        public CategorisationValidatorTests()
        {
            _sut = new CategorisationValidator();
        }

        private const string StringWithTags = "Some string with <tag> in it.";

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void CategorisationShouldErrorWithNoCategoryValue(string value)
        {
            var model = new Categorisation() { Category = value };
            var result = _sut.TestValidate(model);
            result.ShouldNotHaveValidationErrorFor(x => x.Category);
        }

        [Fact]
        public void CategorisationShouldErrorWithTagsInCategory()
        {
            var model = new Categorisation() { Category = StringWithTags };
            var result = _sut.TestValidate(model);
            result.ShouldHaveValidationErrorFor(x => x.Category)
                .WithErrorCode(ErrorCodes.XssCheckFailure);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void CategorisationShouldErrorWithNoSubCategoryValue(string value)
        {
            var model = new Categorisation() { SubCategory = value };
            var result = _sut.TestValidate(model);
            result.ShouldNotHaveValidationErrorFor(x => x.SubCategory);
        }

        [Fact]
        public void CategorisationShouldErrorWithTagsInSubCategory()
        {
            var model = new Categorisation() { SubCategory = StringWithTags };
            var result = _sut.TestValidate(model);
            result.ShouldHaveValidationErrorFor(x => x.SubCategory)
                .WithErrorCode(ErrorCodes.XssCheckFailure);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void CategorisationShouldErrorWithNoDescriptionValue(string value)
        {
            var model = new Categorisation() { Description = value };
            var result = _sut.TestValidate(model);
            result.ShouldNotHaveValidationErrorFor(x => x.Description);
        }

        [Fact]
        public void CategorisationShouldErrorWithTagsInDescription()
        {
            var model = new Categorisation() { Description = StringWithTags };
            var result = _sut.TestValidate(model);
            result.ShouldHaveValidationErrorFor(x => x.Description)
                .WithErrorCode(ErrorCodes.XssCheckFailure);
        }
    }
}
