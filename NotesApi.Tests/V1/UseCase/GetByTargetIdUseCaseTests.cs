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
using NotesApi.V1;
using NotesApi.V1.Domain.Queries;

namespace NotesApi.Tests.V1.UseCase
{
    public class GetByTargetIdUseCaseTests
    {
        private readonly Fixture _fixture = new Fixture();
        private readonly Mock<INotesGateway> _mockGateway;
        private readonly GetByTargetIdUseCase _classUnderTest;

        public GetByTargetIdUseCaseTests()
        {
            _mockGateway = new Mock<INotesGateway>();
            _classUnderTest = new GetByTargetIdUseCase(_mockGateway.Object);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("some-value")]
        public async Task GetByTargetIdUseCaseGatewayReturnsNullReturnsEmptyList(string paginationToken)
        {
            // Arrange
            var id = Guid.NewGuid();
            var query = new GetNotesByTargetIdQuery { TargetId = id, PaginationToken = paginationToken };
            var gatewayResult = new PagedResult<Note>(null, new PaginationDetails(paginationToken));
            _mockGateway.Setup(x => x.GetByTargetIdAsync(query)).ReturnsAsync(gatewayResult);

            // Act
            var response = await _classUnderTest.ExecuteAsync(query).ConfigureAwait(false);

            // Assert
            response.Results.Should().BeEmpty();
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("some-value")]
        public async Task GetByTargetIdUseCaseGatewayReturnsEmptyReturnsEmptyList(string paginationToken)
        {
            // Arrange
            var id = Guid.NewGuid();
            var query = new GetNotesByTargetIdQuery { TargetId = id, PaginationToken = paginationToken };
            var gatewayResult = new PagedResult<Note>(new List<Note>());
            _mockGateway.Setup(x => x.GetByTargetIdAsync(query)).ReturnsAsync(gatewayResult);

            // Act
            var response = await _classUnderTest.ExecuteAsync(query).ConfigureAwait(false);

            // Assert
            response.Results.Should().BeEmpty();
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("some-value")]
        public async Task GetByTargetIdUseCaseGatewayReturnsListReturnsResponseList(string paginationToken)
        {
            // Arrange
            var id = Guid.NewGuid();
            var query = new GetNotesByTargetIdQuery { TargetId = id, PaginationToken = paginationToken };
            var notes = _fixture.CreateMany<Note>(5).ToList();
            var gatewayResult = new PagedResult<Note>(notes, new PaginationDetails(paginationToken));
            _mockGateway.Setup(x => x.GetByTargetIdAsync(query)).ReturnsAsync(gatewayResult);

            // Act
            var response = await _classUnderTest.ExecuteAsync(query).ConfigureAwait(false);

            // Assert
            response.Results.Should().BeEquivalentTo(notes.ToResponse());
            if (string.IsNullOrEmpty(paginationToken))
                response.PaginationDetails.MoreToken.Should().BeNull();
            else
                response.PaginationDetails.DecodeMoreToken().Should().Be(paginationToken);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("some-value")]
        public void GetByTargetIdExceptionIsThrown(string paginationToken)
        {
            // Arrange
            var id = Guid.NewGuid();
            var query = new GetNotesByTargetIdQuery { TargetId = id, PaginationToken = paginationToken };
            var exception = new ApplicationException("Test exception");
            _mockGateway.Setup(x => x.GetByTargetIdAsync(query)).ThrowsAsync(exception);

            // Act
            Func<Task<PagedResult<NoteResponseObject>>> func = async () =>
                await _classUnderTest.ExecuteAsync(query).ConfigureAwait(false);

            // Assert
            func.Should().Throw<ApplicationException>().WithMessage(exception.Message);
        }
    }
}
