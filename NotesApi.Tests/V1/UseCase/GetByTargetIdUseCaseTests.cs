using NotesApi.V1.Gateways;
using NotesApi.V1.UseCase;
using Moq;
using Xunit;
using AutoFixture;
using System.Threading.Tasks;
using System;
using NotesApi.V1.Domain;
using System.Collections.Generic;
using FluentAssertions;
using System.Linq;
using NotesApi.V1.Factories;
using NotesApi.V1.Boundary.Response;

namespace NotesApi.Tests.V1.UseCase
{
    public class GetByTargetIdUseCaseTests
    {
        private readonly Fixture _fixture = new Fixture();
        private readonly Mock<INotesApiGateway> _mockGateway;
        private readonly GetByTargetIdUseCase _classUnderTest;

        public GetByTargetIdUseCaseTests()
        {
            _mockGateway = new Mock<INotesApiGateway>();
            _classUnderTest = new GetByTargetIdUseCase(_mockGateway.Object);
        }

        [Fact]
        public async Task GetByTargetIdUseCaseGatewayReturnsNullReturnsEmptyList()
        {
            // Arrange
            var id = Guid.NewGuid();
            _mockGateway.Setup(x => x.GetByTargetIdAsync(id)).ReturnsAsync((List<Note>) null);

            // Act
            var response = await _classUnderTest.ExecuteAsync(id).ConfigureAwait(false);

            // Assert
            response.Should().BeEmpty();
        }

        [Fact]
        public async Task GetByTargetIdUseCaseGatewayReturnsEmptyReturnsEmptyList()
        {
            // Arrange
            var id = Guid.NewGuid();
            _mockGateway.Setup(x => x.GetByTargetIdAsync(id)).ReturnsAsync(new List<Note>());

            // Act
            var response = await _classUnderTest.ExecuteAsync(id).ConfigureAwait(false);

            // Assert
            response.Should().BeEmpty();
        }

        [Fact]
        public async Task GetByTargetIdUseCaseGatewayReturnsListReturnsResponseList()
        {
            // Arrange
            var id = Guid.NewGuid();
            var notes = _fixture.CreateMany<Note>(5).ToList();
            _mockGateway.Setup(x => x.GetByTargetIdAsync(id)).ReturnsAsync(notes);

            // Act
            var response = await _classUnderTest.ExecuteAsync(id).ConfigureAwait(false);

            // Assert
            response.Should().BeEquivalentTo(notes.ToResponse());
        }

        [Fact]
        public void GetByTargetIdExceptionIsThrown()
        {
            // Arrange
            var id = Guid.NewGuid();
            var exception = new ApplicationException("Test exception");
            _mockGateway.Setup(x => x.GetByTargetIdAsync(id)).ThrowsAsync(exception);

            // Act
            Func<Task<List<NoteResponseObject>>> func = async () => await _classUnderTest.ExecuteAsync(id).ConfigureAwait(false);

            // Assert
            func.Should().Throw<ApplicationException>().WithMessage(exception.Message);
        }
    }
}
