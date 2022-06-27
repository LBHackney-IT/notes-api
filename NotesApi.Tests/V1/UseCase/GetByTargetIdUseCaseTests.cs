using AutoFixture;
using FluentAssertions;
using Hackney.Core.DynamoDb;
using Moq;
using NotesApi.V1.Boundary.Request;
using NotesApi.V1.Boundary.Response;
using NotesApi.V1.Domain;
using NotesApi.V1.Factories;
using NotesApi.V1.Gateways;
using NotesApi.V1.UseCase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NotesApi.V1.Infrastructure;
using Xunit;

namespace NotesApi.Tests.V1.UseCase
{
    [Collection("LogCall collection")]
    public class GetByTargetIdUseCaseTests
    {
        private readonly Fixture _fixture = new Fixture();
        private readonly Mock<INotesGateway> _mockGateway;
        private readonly Mock<IExcludedCategoriesFactory> _mockExcludedCategoriesFactory;
        private readonly GetByTargetIdUseCase _classUnderTest;

        public GetByTargetIdUseCaseTests()
        {
            _mockGateway = new Mock<INotesGateway>();
            _mockExcludedCategoriesFactory = new Mock<IExcludedCategoriesFactory>();

            _classUnderTest = new GetByTargetIdUseCase(_mockGateway.Object, _mockExcludedCategoriesFactory.Object);
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
            _mockGateway.Setup(x => x.GetByTargetIdAsync(query, It.IsAny<List<ExcludedCategory>>())).ReturnsAsync(gatewayResult);

            // Act
            var response = await _classUnderTest.ExecuteAsync(query, new List<string>()).ConfigureAwait(false);

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
            _mockGateway.Setup(x => x.GetByTargetIdAsync(query, It.IsAny<List<ExcludedCategory>>())).ReturnsAsync(gatewayResult);

            // Act
            var response = await _classUnderTest.ExecuteAsync(query, new List<string>()).ConfigureAwait(false);

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
            _mockGateway.Setup(x => x.GetByTargetIdAsync(query, It.IsAny<List<ExcludedCategory>>())).ReturnsAsync(gatewayResult);

            // Act
            var response = await _classUnderTest.ExecuteAsync(query, new List<string>()).ConfigureAwait(false);

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
            _mockGateway.Setup(x => x.GetByTargetIdAsync(query, It.IsAny<List<ExcludedCategory>>())).ThrowsAsync(exception);

            // Act
            Func<Task<PagedResult<NoteResponseObject>>> func = async () =>
                await _classUnderTest.ExecuteAsync(query, new List<string>()).ConfigureAwait(false);

            // Assert
            func.Should().Throw<ApplicationException>().WithMessage(exception.Message);
        }
    }
}
