using System.Collections.Generic;
using Hackney.Core.DynamoDb;
using NotesApi.V1.Boundary.Request;
using NotesApi.V1.Domain;
using System.Threading.Tasks;
using NotesApi.V1.Infrastructure;

namespace NotesApi.V1.Gateways
{
    public interface INotesGateway
    {
        Task<PagedResult<Note>> GetByTargetIdAsync(GetNotesByTargetIdQuery query, List<ExcludedCategory> excludedCategories = null);

        Task<Note> PostNewNoteAsync(CreateNoteRequest request);
    }
}
