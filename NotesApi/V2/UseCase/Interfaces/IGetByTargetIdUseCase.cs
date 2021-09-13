using System.Threading.Tasks;
using Hackney.Core.DynamoDb;
using NotesApi.V2.Boundary.Request;
using NotesApi.V2.Boundary.Response;

namespace NotesApi.V2.UseCase.Interfaces
{
    public interface IGetByTargetIdUseCase
    {
        Task<PagedResult<NoteResponseObject>> ExecuteAsync(GetNotesByTargetIdQuery query);
    }
}
