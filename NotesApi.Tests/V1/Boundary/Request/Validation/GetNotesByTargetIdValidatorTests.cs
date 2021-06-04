using FluentValidation.TestHelper;
using NotesApi.V1.Boundary.Request;
using NotesApi.V1.Boundary.Request.Validation;
using System;
using Xunit;

namespace NotesApi.Tests.V1.Boundary.Request.Validation
{
    public class GetNotesByTargetIdValidatorTests
    {
        private readonly GetNotesByTargetIdValidator _sut;

        public GetNotesByTargetIdValidatorTests()
        {
            _sut = new GetNotesByTargetIdValidator();
        }

        [Fact]
        public void QueryShouldErrorWithNullTargetId()
        {
            var query = new GetNotesByTargetIdQuery();
            var result = _sut.TestValidate(query);
            result.ShouldHaveValidationErrorFor(x => x.TargetId);
        }

        [Fact]
        public void QueryShouldErrorWithEmptyTargetId()
        {
            var query = new GetNotesByTargetIdQuery() { TargetId = Guid.Empty };
            var result = _sut.TestValidate(query);
            result.ShouldHaveValidationErrorFor(x => x.TargetId);
        }

        [Fact]
        public void QueryShouldNotErrorWithNoPageSize()
        {
            var query = new GetNotesByTargetIdQuery() { TargetId = Guid.NewGuid() };
            var result = _sut.TestValidate(query);
            result.ShouldNotHaveValidationErrorFor(x => x.PageSize);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-10)]
        public void QueryShouldErrorWithInvalidPageSize(int invalidPageSize)
        {
            var query = new GetNotesByTargetIdQuery() { TargetId = Guid.NewGuid(), PageSize = invalidPageSize };
            var result = _sut.TestValidate(query);
            result.ShouldHaveValidationErrorFor(x => x.PageSize);
        }
    }
}
