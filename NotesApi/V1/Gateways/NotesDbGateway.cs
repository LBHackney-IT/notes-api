using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.DocumentModel;
using NotesApi.V1.Domain;
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
        private readonly IDynamoDBContext _dynamoDbContext;

        public NotesDbGateway(IDynamoDBContext dynamoDbContext)
        {
            _dynamoDbContext = dynamoDbContext;
        }

        // This method cannot be unit tested because of having to use GetTargetTable()
        // which returns an unmockable concrete class.
        // See here: https://github.com/aws/aws-sdk-net/issues/1310
        [ExcludeFromCodeCoverage]
        public async Task<IEnumerable<Note>> GetByTargetIdAsync(Guid targetId)
        {
            var dbNotes = new List<NoteDb>();
            var table = _dynamoDbContext.GetTargetTable<NoteDb>();
            var search = table.Query(new QueryOperationConfig
            {
                IndexName = "NotesByDate",
                BackwardSearch = true,
                ConsistentRead = true,
                Filter = new QueryFilter(nameof(targetId), QueryOperator.Equal, targetId)
            });
            var resultsSet = await search.GetNextSetAsync().ConfigureAwait(false);
            while (resultsSet.Any())
            {
                dbNotes.AddRange(_dynamoDbContext.FromDocuments<NoteDb>(resultsSet));
                resultsSet = await search.GetNextSetAsync().ConfigureAwait(false);
            }

            return dbNotes.Select(x => x.ToDomain()).ToList();
        }
    }
}
