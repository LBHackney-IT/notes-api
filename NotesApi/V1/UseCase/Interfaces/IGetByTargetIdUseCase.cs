using System.Collections.Generic;
using Hackney.Core.DynamoDb;
using NotesApi.V1.Boundary.Request;
using NotesApi.V1.Boundary.Response;
using System.Threading.Tasks;

namespace NotesApi.V1.UseCase.Interfaces
{
    public interface IGetByTargetIdUseCase
    {
        Task<PagedResult<NoteResponseObject>> ExecuteAsync(GetNotesByTargetIdQuery query, List<string> excludedCategoryNames);
    }
}
