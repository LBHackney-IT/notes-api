using AutoFixture;
using FluentAssertions;
using Hackney.Core.Testing.DynamoDb;
using Hackney.Core.Testing.Shared;
using Microsoft.Extensions.Logging;
using Moq;
using NotesApi.V2.Boundary.Request;
using NotesApi.V2.Domain;
using NotesApi.V2.Gateways;
using NotesApi.V2.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace NotesApi.Tests.V2.Gateways
{
    [Collection("AppTest collection")]
    public class NotesDbGatewayTests : IDisposable
    {
        private readonly Fixture _fixture = new Fixture();
        private readonly Mock<ILogger<NotesDbGateway>> _logger;
        private readonly IDynamoDbFixture _dbFixture;
        private readonly NotesDbGateway _classUnderTest;
        private readonly List<Action> _cleanup = new List<Action>();

        public NotesDbGatewayTests(MockWebApplicationFactory<Startup> appFactory)
        {
            _logger = new Mock<ILogger<NotesDbGateway>>();
            _dbFixture = appFactory.DynamoDbFixture;
            _classUnderTest = new NotesDbGateway(_dbFixture.DynamoDbContext, _logger.Object);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private bool _disposed;
        protected virtual void Dispose(bool disposing)
        {
            if (disposing && !_disposed)
            {
                foreach (var action in _cleanup)
                    action();

                _disposed = true;
            }
        }

        private List<NoteDb> UpsertNotes(Guid targetId, int count)
        {
            var notes = new List<NoteDb>();

            var random = new Random();
            Func<DateTime> funcDT = () => DateTime.UtcNow.AddDays(0 - random.Next(100));
            notes.AddRange(_fixture.Build<NoteDb>()
                                   .With(x => x.CreatedAt, funcDT)
                                   .With(x => x.TargetType, TargetType.person)
                                   .With(x => x.TargetId, targetId)
                                   .CreateMany(count));

            foreach (var note in notes)
            {
                _dbFixture.SaveEntityAsync(note).GetAwaiter().GetResult();
            }

            return notes;
        }

        #region GetByTargetId Tests

        [Fact]
        public async Task GetByTargetIdReturnsEmptyIfNoRecords()
        {
            var query = new GetNotesByTargetIdQuery() { TargetId = Guid.NewGuid() };
            var response = await _classUnderTest.GetByTargetIdAsync(query).ConfigureAwait(false);
            response.Should().NotBeNull();
            response.Results.Should().BeEmpty();
            response.PaginationDetails.HasNext.Should().BeFalse();
            response.PaginationDetails.NextToken.Should().BeNull();

            _logger.VerifyExact(LogLevel.Debug, $"Querying NotesByCreated index for targetId {query.TargetId}", Times.Once());
        }

        [Fact]
        public async Task GetByTargetIdReturnsRecords()
        {
            var targetId = Guid.NewGuid();
            var expected = UpsertNotes(targetId, 5);

            var query = new GetNotesByTargetIdQuery() { TargetId = targetId };
            var response = await _classUnderTest.GetByTargetIdAsync(query).ConfigureAwait(false);
            response.Should().NotBeNull();
            response.Results.Should().BeEquivalentTo(expected);
            response.PaginationDetails.HasNext.Should().BeFalse();
            response.PaginationDetails.NextToken.Should().BeNull();

            _logger.VerifyExact(LogLevel.Debug, $"Querying NotesByCreated index for targetId {query.TargetId}", Times.Once());
        }

        [Fact]
        public async Task GetByTargetIdReturnsRecordsAllPages()
        {
            var targetId = Guid.NewGuid();
            var expected = UpsertNotes(targetId, 9);
            var expectedFirstPage = expected.OrderByDescending(x => x.CreatedAt).Take(5);
            var expectedSecondPage = expected.Except(expectedFirstPage).OrderByDescending(x => x.CreatedAt);

            var query = new GetNotesByTargetIdQuery() { TargetId = targetId, PageSize = 5 };
            var response = await _classUnderTest.GetByTargetIdAsync(query).ConfigureAwait(false);
            response.Should().NotBeNull();
            response.Results.Should().BeEquivalentTo(expectedFirstPage);
            response.PaginationDetails.HasNext.Should().BeTrue();
            response.PaginationDetails.NextToken.Should().NotBeNull();

            query.PaginationToken = response.PaginationDetails.NextToken;
            response = await _classUnderTest.GetByTargetIdAsync(query).ConfigureAwait(false);
            response.Should().NotBeNull();
            response.Results.Should().BeEquivalentTo(expectedSecondPage);
            response.PaginationDetails.HasNext.Should().BeFalse();
            response.PaginationDetails.NextToken.Should().BeNull();

            _logger.VerifyExact(LogLevel.Debug, $"Querying NotesByCreated index for targetId {query.TargetId}", Times.Exactly(2));
        }

        [Fact]
        public async Task GetByTargetIdReturnsNoPaginationTokenIfPageSizeEqualsRecordCount()
        {
            var targetId = Guid.NewGuid();
            var expected = UpsertNotes(targetId, 10);

            var query = new GetNotesByTargetIdQuery() { TargetId = targetId, PageSize = 10 };
            var response = await _classUnderTest.GetByTargetIdAsync(query).ConfigureAwait(false);
            response.Should().NotBeNull();
            response.Results.Should().BeEquivalentTo(expected);
            response.PaginationDetails.HasNext.Should().BeFalse();
            response.PaginationDetails.NextToken.Should().BeNull();

            _logger.VerifyExact(LogLevel.Debug, $"Querying NotesByCreated index for targetId {query.TargetId}", Times.Once());
        }

        #endregion GetByTargetId Tests
    }
}
