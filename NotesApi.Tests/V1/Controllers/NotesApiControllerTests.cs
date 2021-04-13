using AutoFixture;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NotesApi.V1.Boundary.Response;
using NotesApi.V1.Controllers;
using NotesApi.V1.UseCase.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace NotesApi.Tests.V1.Controllers
{
    public class NotesApiControllerTests
    {
        private readonly Mock<IGetByTargetIdUseCase> _mockGetByTargetIdUseCase;
        private readonly NotesApiController _sut;
        private readonly Fixture _fixture = new Fixture();

        public NotesApiControllerTests()
        {
            _mockGetByTargetIdUseCase = new Mock<IGetByTargetIdUseCase>();
            _sut = new NotesApiController(_mockGetByTargetIdUseCase.Object);
        }

        [Fact]
        public async Task GetPersonByIdAsyncNotFoundReturnsNotFound()
        {
            // Arrange
            var id = Guid.NewGuid();
            _mockGetByTargetIdUseCase.Setup(x => x.ExecuteAsync(id)).ReturnsAsync((List<NoteResponseObject>) null);

            // Act
            var response = await _sut.GetByTargetIdAsync(id).ConfigureAwait(false);

            // Assert
            response.Should().BeOfType(typeof(NotFoundObjectResult));
            (response as NotFoundObjectResult).Value.Should().Be(id);
        }

        [Fact]
        public async Task GetPersonByIdAsyncFoundReturnsResponse()
        {
            // Arrange
            var id = Guid.NewGuid();
            var notesResponse = _fixture.CreateMany<NoteResponseObject>(5).ToList();
            _mockGetByTargetIdUseCase.Setup(x => x.ExecuteAsync(id)).ReturnsAsync(notesResponse);

            // Act
            var response = await _sut.GetByTargetIdAsync(id).ConfigureAwait(false);

            // Assert
            response.Should().BeOfType(typeof(OkObjectResult));
            (response as OkObjectResult).Value.Should().BeEquivalentTo(notesResponse);
        }

        [Fact]
        public void GetPersonByIdAsyncExceptionIsThrown()
        {
            // Arrange
            var id = Guid.NewGuid();
            var exception = new ApplicationException("Test exception");
            _mockGetByTargetIdUseCase.Setup(x => x.ExecuteAsync(id)).ThrowsAsync(exception);

            // Act
            Func<Task<IActionResult>> func = async () => await _sut.GetByTargetIdAsync(id).ConfigureAwait(false);

            // Assert
            func.Should().Throw<ApplicationException>().WithMessage(exception.Message);
        }
    }
}
