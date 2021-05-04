using AutoFixture;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NotesApi.V1;
using NotesApi.V1.Boundary.Response;
using NotesApi.V1.Controllers;
using NotesApi.V1.Domain.Queries;
using NotesApi.V1.UseCase.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NotesApi.V1.Boundary;
using Xunit;

namespace NotesApi.Tests.V1.Controllers
{
    [Collection("LogCall collection")]
    public class NotesApiControllerTests
    {
        private readonly Mock<IGetByTargetIdUseCase> _mockGetByTargetIdUseCase;
        private readonly Mock<IPostNewNoteUseCase> _mockPostNewNoteUseCase;
        private readonly NotesApiController _sut;
        private readonly Fixture _fixture = new Fixture();

        public NotesApiControllerTests()
        {
            _mockGetByTargetIdUseCase = new Mock<IGetByTargetIdUseCase>();
            _mockPostNewNoteUseCase = new Mock<IPostNewNoteUseCase>();

            _sut = new NotesApiController(_mockGetByTargetIdUseCase.Object, _mockPostNewNoteUseCase.Object);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("some-value")]
        public async Task GetPersonByIdAsyncNotFoundReturnsNotFound(string paginationToken)
        {
            // Arrange
            var id = Guid.NewGuid();
            var query = new GetNotesByTargetIdQuery { TargetId = id, PaginationToken = paginationToken };
            _mockGetByTargetIdUseCase.Setup(x => x.ExecuteAsync(query)).ReturnsAsync((PagedResult<NoteResponseObject>) null);

            // Act
            var response = await _sut.GetByTargetIdAsync(query).ConfigureAwait(false);

            // Assert
            response.Should().BeOfType(typeof(NotFoundObjectResult));
            (response as NotFoundObjectResult).Value.Should().Be(id);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("some-value")]
        public async Task GetPersonByIdAsyncFoundReturnsResponse(string paginationToken)
        {
            // Arrange
            var id = Guid.NewGuid();
            var query = new GetNotesByTargetIdQuery { TargetId = id, PaginationToken = paginationToken };
            var notes = _fixture.CreateMany<NoteResponseObject>(5).ToList();
            var pagedResult = new PagedResult<NoteResponseObject>(notes, new PaginationDetails(paginationToken));
            _mockGetByTargetIdUseCase.Setup(x => x.ExecuteAsync(query)).ReturnsAsync(pagedResult);

            // Act
            var response = await _sut.GetByTargetIdAsync(query).ConfigureAwait(false);

            // Assert
            response.Should().BeOfType(typeof(OkObjectResult));
            (response as OkObjectResult).Value.Should().BeEquivalentTo(pagedResult);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("some-value")]
        public void GetPersonByIdAsyncExceptionIsThrown(string paginationToken)
        {
            // Arrange
            var id = Guid.NewGuid();
            var query = new GetNotesByTargetIdQuery { TargetId = id, PaginationToken = paginationToken };
            var exception = new ApplicationException("Test exception");
            _mockGetByTargetIdUseCase.Setup(x => x.ExecuteAsync(query)).ThrowsAsync(exception);

            // Act
            Func<Task<IActionResult>> func = async () => await _sut.GetByTargetIdAsync(query).ConfigureAwait(false);

            // Assert
            func.Should().Throw<ApplicationException>().WithMessage(exception.Message);
        }

        [Fact]
        public void PostNewNoteAsyncExceptionIsThrown()
        {
            // Arrange
            var exception = new ApplicationException("Test exception");
            _mockPostNewNoteUseCase.Setup(x => x.ExecuteAsync(It.IsAny<CreateNoteRequest>())).ThrowsAsync(exception);

            // Act
            Func<Task<IActionResult>> func = async () => await _sut.PostNewNote(new CreateNoteRequest()).ConfigureAwait(false);

            // Assert
            func.Should().Throw<ApplicationException>().WithMessage(exception.Message);
        }

        [Fact]
        public async Task PostNewNoteReturnsCreated()
        {
            // Arrange
            var newNote = _fixture.Create<NoteResponseObject>();
            _mockPostNewNoteUseCase.Setup(x => x.ExecuteAsync(It.IsAny<CreateNoteRequest>())).
                ReturnsAsync(newNote);

            // Act
            var result = await _sut.PostNewNote(new CreateNoteRequest()).ConfigureAwait(false);

            // Assert
            (result as CreatedResult).Should().NotBe(null);
            (result as CreatedResult).Value.Should().Be(newNote);
        }
    }
}
