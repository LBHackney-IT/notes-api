using System.Threading.Tasks;
using NotesApi.V1.Boundary.Response;
using NotesApi.V1.Domain.Queries;

namespace NotesApi.V1.UseCase.Interfaces
{
    public interface IPostNewNoteUseCase
    {
        Task<NoteResponseObject> ExecuteAsync(CreateNoteRequest createNoteRequest);
    }
}
