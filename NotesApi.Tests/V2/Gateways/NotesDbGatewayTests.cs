using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Amazon.DynamoDBv2.DataModel;
using AutoFixture;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using NotesApi.V2.Boundary.Request;
using NotesApi.V2.Domain;
using NotesApi.V2.Gateways;
using NotesApi.V2.Infrastructure;
using Xunit;

namespace NotesApi.Tests.V2.Gateways
{
    [Collection("DynamoDb collection")]
    public class NotesDbGatewayTests : IDisposable
    {
        private readonly Fixture _fixture = new Fixture();
        private readonly Mock<ILogger<NotesDbGateway>> _logger;
        private readonly IDynamoDBContext _dynamoDb;
        private readonly NotesDbGateway _classUnderTest;
        private readonly List<Action> _cleanup = new List<Action>();

        public NotesDbGatewayTests(DynamoDbIntegrationTests<Startup> dbTestFixture)
        {
            _logger = new Mock<ILogger<NotesDbGateway>>();
            _dynamoDb = dbTestFixture.DynamoDbContext;
            _classUnderTest = new NotesDbGateway(_dynamoDb, _logger.Object);
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
                _dynamoDb.SaveAsync(note).GetAwaiter().GetResult();
                _cleanup.Add(async () => await _dynamoDb.DeleteAsync(note, default).ConfigureAwait(false));
            }

            return notes;
        }

        private static CreateNoteRequest CreateNoteRequest()
        {
            var note = new Fixture().Create<CreateNoteRequest>();

            note.TargetId = Guid.NewGuid();
            note.CreatedAt = DateTime.Now;
            note.Author.Email = "something@somewhere.com";
            return note;
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

        #region PostNewNote Tests

        [Fact]
        public async Task PostNewNoteReturnsNote()
        {
            var request = CreateNoteRequest();
            var response = await _classUnderTest.PostNewNoteAsync(request).ConfigureAwait(false);
            request.Should().NotBeNull();
            _cleanup.Add(async () =>
                await _dynamoDb.DeleteAsync<NoteDb>(response.TargetId, response.Id, default).ConfigureAwait(false));

            request.Should().BeEquivalentTo(response, (opt) => opt.Excluding(x => x.Id));
            response.Id.Should().NotBeEmpty();

            _logger.VerifyExact(LogLevel.Debug,
                $"Saving a new note for targetId: {request.TargetId}, targetType: {Enum.GetName(typeof(TargetType), request.TargetType)}",
                Times.Once());
        }

        [Fact]
        public async Task PostNewNoteNoCategoryOrAuthorReturnsNote()
        {
            var request = CreateNoteRequest();
            request.Author = null;
            request.Categorisation = null;
            var response = await _classUnderTest.PostNewNoteAsync(request).ConfigureAwait(false);
            request.Should().NotBeNull();
            _cleanup.Add(async () =>
                await _dynamoDb.DeleteAsync<NoteDb>(response.TargetId, response.Id, default).ConfigureAwait(false));

            request.Should().BeEquivalentTo(response, (opt) => opt.Excluding(x => x.Id)
                                                      .Excluding(y => y.Author)
                                                      .Excluding(z => z.Categorisation));
            response.Id.Should().NotBeEmpty();
            response.Author.Should().BeEquivalentTo(new AuthorDetails());
            response.Categorisation.Should().BeEquivalentTo(new Categorisation());

            _logger.VerifyExact(LogLevel.Debug,
                $"Saving a new note for targetId: {request.TargetId}, targetType: {Enum.GetName(typeof(TargetType), request.TargetType)}",
                Times.Once());
        }

        #endregion PostNewNote Tests
    }
}
