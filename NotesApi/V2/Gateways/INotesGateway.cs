using System.Threading.Tasks;
using Hackney.Core.DynamoDb;
using NotesApi.V2.Boundary.Request;
using NotesApi.V2.Domain;

namespace NotesApi.V2.Gateways
{
    public interface INotesGateway
    {
        Task<PagedResult<Note>> GetByTargetIdAsync(GetNotesByTargetIdQuery query);

        Task<Note> PostNewNoteAsync(CreateNoteRequest request);
    }
}
