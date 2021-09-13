using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoFixture;
using FluentAssertions;
using Hackney.Core.DynamoDb;
using Moq;
using NotesApi.V2.Boundary.Request;
using NotesApi.V2.Boundary.Response;
using NotesApi.V2.Domain;
using NotesApi.V2.Factories;
using NotesApi.V2.Gateways;
using NotesApi.V2.UseCase;
using Xunit;

namespace NotesApi.Tests.V2.UseCase
{
    [Collection("LogCall collection")]
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
                response.PaginationDetails.NextToken.Should().BeNull();
            else
                response.PaginationDetails.DecodeNextToken().Should().Be(paginationToken);
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
