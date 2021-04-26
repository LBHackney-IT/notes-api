using System.Threading.Tasks;
using NotesApi.V1.Boundary;
using NotesApi.V1.Boundary.Response;

namespace NotesApi.V1.UseCase.Interfaces
{
    public interface IPostNewNoteUseCase
    {
        Task<NoteResponseObject> ExecuteAsync(CreateNoteRequest createNoteRequest);
    }
}
