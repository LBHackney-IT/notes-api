using System;
using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.DocumentModel;
using NotesApi.V1.Domain;
using NotesApi.V1.Domain.Queries;
using NotesApi.V1.Factories;
using NotesApi.V1.Infrastructure;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using NotesApi.V1.Boundary;

namespace NotesApi.V1.Gateways
{
    public class NotesDbGateway : INotesGateway
    {
        private const int MAX_RESULTS = 10;
        private const string GETNOTESBYTARGETIDINDEX = "NotesByDate";
        private const string TARGETID = "targetId";

        private readonly IDynamoDBContext _dynamoDbContext;

        public NotesDbGateway(IDynamoDBContext dynamoDbContext)
        {
            _dynamoDbContext = dynamoDbContext;
        }

        // This method cannot be unit tested because of having to use GetTargetTable()
        // which returns an unmockable concrete class.
        // See here: https://github.com/aws/aws-sdk-net/issues/1310
        [ExcludeFromCodeCoverage]
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
            var resultsSet = await search.GetNextSetAsync().ConfigureAwait(false);
            if (resultsSet.Any())
                dbNotes.AddRange(_dynamoDbContext.FromDocuments<NoteDb>(resultsSet));

            return new PagedResult<Note>(dbNotes.Select(x => x.ToDomain()), new PaginationDetails(search.PaginationToken));
        }

        public async Task<Note> PostNewNoteAsync(CreateNoteRequest request)
        {
            var dbNote = new NoteDb
            {
                Id = Guid.NewGuid(),
                Description = request.Description,
                DateTime = request.CreatedAt,
                TargetId = Guid.Parse(request.TargetId),
                Author = new AuthorDetails
                {
                    Id = request.Author.Id,
                    Email = request.Author.Email,
                    FullName = request.Author.FullName
                },
                Categorisation = new Categorisation
                {
                    Description = request.Categorisation.Description,
                    Category = request.Categorisation.Category,
                    SubCategory = request.Categorisation.SubCategory
                }
            };

            await _dynamoDbContext.SaveAsync(dbNote).ConfigureAwait(false);

            return dbNote.ToDomain();
        }
    }
}
