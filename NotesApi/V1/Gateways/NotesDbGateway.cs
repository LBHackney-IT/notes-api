using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.DocumentModel;
using Hackney.Core.DynamoDb;
using Hackney.Core.Logging;
using Microsoft.Extensions.Logging;
using NotesApi.V1.Boundary.Request;
using NotesApi.V1.Domain;
using NotesApi.V1.Factories;
using NotesApi.V1.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NotesApi.V1.Gateways
{
    public class NotesDbGateway : INotesGateway
    {
        private const int MAX_RESULTS = 10;
        private const string GETNOTESBYTARGETIDINDEX = "NotesByCreated";

        private readonly IDynamoDBContext _dynamoDbContext;
        private readonly ILogger<NotesDbGateway> _logger;

        public NotesDbGateway(IDynamoDBContext dynamoDbContext, ILogger<NotesDbGateway> logger)
        {
            _dynamoDbContext = dynamoDbContext;
            _logger = logger;
        }

        [LogCall]
        public async Task<PagedResult<Note>> GetByTargetIdAsync(GetNotesByTargetIdQuery query, string categoryToExclude = null)
        {
            int pageSize = query.PageSize.HasValue ? query.PageSize.Value : MAX_RESULTS;
            var dbNotes = new List<NoteDb>();
            var table = _dynamoDbContext.GetTargetTable<NoteDb>();

            var filterExpression = new Expression();
            filterExpression.ExpressionAttributeNames.Add("#t", "targetId");
            filterExpression.ExpressionAttributeValues.Add(":targetId", query.TargetId);

            if (string.IsNullOrEmpty(categoryToExclude))
            {
                filterExpression.ExpressionAttributeNames.Add("#c", "categorisation.category");
                filterExpression.ExpressionAttributeValues.Add(":cat", categoryToExclude);
                filterExpression.ExpressionStatement = "#c <> :cat";
            }

            var keyExpression = new Expression();
            keyExpression.ExpressionStatement = "#t = :targetId";

            var queryConfig = new QueryOperationConfig
            {
                IndexName = GETNOTESBYTARGETIDINDEX,
                BackwardSearch = true,
                ConsistentRead = true,
                Limit = pageSize,
                PaginationToken = PaginationDetails.DecodeToken(query.PaginationToken),
                FilterExpression = filterExpression,
                KeyExpression = keyExpression
            };

            var search = table.Query(queryConfig);

            _logger.LogDebug($"Querying {queryConfig.IndexName} index for targetId {query.TargetId}");
            var resultsSet = await search.GetNextSetAsync().ConfigureAwait(false);

            var paginationToken = search.PaginationToken;
            if (resultsSet.Any())
            {
                dbNotes.AddRange(_dynamoDbContext.FromDocuments<NoteDb>(resultsSet));

                // Look ahead for any more, but only if we have a token
                if (!string.IsNullOrEmpty(PaginationDetails.EncodeToken(paginationToken)))
                {
                    queryConfig.PaginationToken = paginationToken;
                    queryConfig.Limit = 1;
                    search = table.Query(queryConfig);
                    resultsSet = await search.GetNextSetAsync().ConfigureAwait(false);
                    if (!resultsSet.Any())
                        paginationToken = null;
                }
            }

            var notes = dbNotes.Select(x => x.ToDomain());
            return new PagedResult<Note>(notes, new PaginationDetails(paginationToken));
        }

        [LogCall]
        public async Task<Note> PostNewNoteAsync(CreateNoteRequest request)
        {
            var dbNote = new NoteDb
            {
                Id = Guid.NewGuid(),
                Description = request.Description,
                CreatedAt = request.CreatedAt.Value,
                TargetId = request.TargetId.Value,
                TargetType = request.TargetType.Value,
                Author = new AuthorDetails
                {
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

            _logger.LogDebug($"Saving a new note for targetId: {dbNote.TargetId}, targetType: {Enum.GetName(typeof(TargetType), dbNote.TargetType)}");
            await _dynamoDbContext.SaveAsync(dbNote).ConfigureAwait(false);
            return dbNote.ToDomain();
        }
    }
}
