using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.DocumentModel;
using Hackney.Core.Logging;
using Microsoft.Extensions.Logging;
using NotesApi.V1.Domain;
using NotesApi.V1.Domain.Queries;
using NotesApi.V1.Factories;
using NotesApi.V1.Infrastructure;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;

namespace NotesApi.V1.Gateways
{
    public class NotesDbGateway : INotesGateway
    {
        private const int MAX_RESULTS = 10;
        private const string GETNOTESBYTARGETIDINDEX = "NotesByDate";
        private const string TARGETID = "targetId";

        private readonly IDynamoDBContext _dynamoDbContext;
        private readonly ILogger<NotesDbGateway> _logger;

        public NotesDbGateway(IDynamoDBContext dynamoDbContext, ILogger<NotesDbGateway> logger)
        {
            _dynamoDbContext = dynamoDbContext;
            _logger = logger;
        }

        // This method cannot be unit tested because of having to use GetTargetTable()
        // which returns an unmockable concrete class.
        // See here: https://github.com/aws/aws-sdk-net/issues/1310
        [ExcludeFromCodeCoverage]
        [LogCall]
        public async Task<PagedResult<Note>> GetByTargetIdAsync(GetNotesByTargetIdQuery query)
        {
            _logger.LogDebug($"Querying IDynamoDBContext.GetTargetTable for TargetId {query.TargetId}");
            int pageSize = query.PageSize.HasValue ? query.PageSize.Value : MAX_RESULTS;
            var dbNotes = new List<NoteDb>();
            var table = _dynamoDbContext.GetTargetTable<NoteDb>();
            var search = table.Query(new QueryOperationConfig
            {
                IndexName = GETNOTESBYTARGETIDINDEX,
                BackwardSearch = true,
                ConsistentRead = true,
                Limit = pageSize,
                PaginationToken = PaginationDetails.DecodeToken(query.PaginationToken),
                Filter = new QueryFilter(TARGETID, QueryOperator.Equal, query.TargetId)
            });
            var resultsSet = await search.GetNextSetAsync().ConfigureAwait(false);
            if (resultsSet.Any())
                dbNotes.AddRange(_dynamoDbContext.FromDocuments<NoteDb>(resultsSet));

            return new PagedResult<Note>(dbNotes.Select(x => x.ToDomain()), new PaginationDetails(search.PaginationToken));
        }

        [LogCall]
        public async Task<Note> PostNewNoteAsync(CreateNoteRequest request)
        {
            var dbNote = new NoteDb
            {
                Id = Guid.NewGuid(),
                Description = request.Description,
                CreatedAt = request.CreatedAt.Value,
                TargetId = request.TargetId,
                TargetType = request.TargetType,
                Author = new AuthorDetails
                {
                    Id = request.Author?.Id,
                    Email = request.Author?.Email,
                    FullName = request.Author?.FullName
                },
                Categorisation = new Categorisation
                {
                    Description = request.Categorisation?.Description,
                    Category = request.Categorisation?.Category,
                    SubCategory = request.Categorisation?.SubCategory
                }
            };

            await _dynamoDbContext.SaveAsync(dbNote).ConfigureAwait(false);

            return dbNote.ToDomain();
        }
    }
}
