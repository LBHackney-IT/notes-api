using Hackney.Core.DynamoDb;
using NotesApi.V1.Boundary.Queries;
using NotesApi.V1.Domain;
using System.Threading.Tasks;

namespace NotesApi.V1.Gateways
{
    public interface INotesGateway
    {
        Task<PagedResult<Note>> GetByTargetIdAsync(GetNotesByTargetIdQuery query);

        Task<Note> PostNewNoteAsync(CreateNoteRequest request);
    }
}
