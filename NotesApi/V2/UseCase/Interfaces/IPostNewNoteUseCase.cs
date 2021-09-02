using System.Threading.Tasks;
using NotesApi.V2.Boundary.Request;
using NotesApi.V2.Boundary.Response;

namespace NotesApi.V2.UseCase.Interfaces
{
    public interface IPostNewNoteUseCase
    {
        Task<NoteResponseObject> ExecuteAsync(CreateNoteRequest createNoteRequest);
    }
}
