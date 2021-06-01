using Hackney.Core.DynamoDb;
using NotesApi.V1.Boundary.Response;
using NotesApi.V1.Domain.Queries;
using System.Threading.Tasks;

namespace NotesApi.V1.UseCase.Interfaces
{
    public interface IGetByTargetIdUseCase
    {
        Task<PagedResult<NoteResponseObject>> ExecuteAsync(GetNotesByTargetIdQuery query);
    }
}
