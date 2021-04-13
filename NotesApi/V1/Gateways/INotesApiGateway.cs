using NotesApi.V1.Domain;
using NotesApi.V1.Domain.Queries;
using System.Threading.Tasks;

namespace NotesApi.V1.Gateways
{
    public interface INotesApiGateway
    {
        Task<PagedResult<Note>> GetByTargetIdAsync(GetNotesByTargetIdQuery query);
    }
}
