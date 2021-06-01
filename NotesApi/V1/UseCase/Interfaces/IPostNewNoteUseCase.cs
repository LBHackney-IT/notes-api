using NotesApi.V1.Boundary.Request;
using NotesApi.V1.Boundary.Response;
using System.Threading.Tasks;

namespace NotesApi.V1.UseCase.Interfaces
{
    public interface IPostNewNoteUseCase
    {
        Task<NoteResponseObject> ExecuteAsync(CreateNoteRequest createNoteRequest);
    }
}
