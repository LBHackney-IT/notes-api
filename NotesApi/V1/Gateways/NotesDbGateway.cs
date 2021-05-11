using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.DocumentModel;
using Microsoft.Extensions.Logging;
using NotesApi.V1.Domain;
using NotesApi.V1.Domain.Queries;
using NotesApi.V1.Factories;
using NotesApi.V1.Infrastructure;
using NotesApi.V1.Logging;
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
        private const string GETNOTESBYTARGETIDINDEX = "NotesByCreated";
        private const string TARGETID = "targetId";

        private readonly IDynamoDBContext _dynamoDbContext;
        private readonly ILogger<NotesDbGateway> _logger;

        public NotesDbGateway(IDynamoDBContext dynamoDbContext, ILogger<NotesDbGateway> logger)
        {
            _dynamoDbContext = dynamoDbContext;
            _logger = logger;
        }

        [LogCall]
        public async Task<PagedResult<Note>> GetByTargetIdAsync(GetNotesByTargetIdQuery query)
        {
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

            _logger.LogDebug($"Querying {GETNOTESBYTARGETIDINDEX} index for targetId {query.TargetId}");
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
                TargetId = request.TargetId.Value,
                TargetType = request.TargetType.Value,
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

            _logger.LogDebug($"Saving a new note for targetId: {dbNote.TargetId}, targetType: {Enum.GetName(typeof(TargetType), dbNote.TargetType)}");
            await _dynamoDbContext.SaveAsync(dbNote).ConfigureAwait(false);
            return dbNote.ToDomain();
        }
    }
}
