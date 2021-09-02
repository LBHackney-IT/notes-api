using System.Threading.Tasks;
using Hackney.Core.Logging;
using NotesApi.V2.Boundary.Request;
using NotesApi.V2.Boundary.Response;
using NotesApi.V2.Gateways;
using NotesApi.V2.UseCase.Interfaces;

namespace NotesApi.V2.UseCase
{
    public class PostNewNoteUseCase : IPostNewNoteUseCase
    {
        private readonly INotesGateway _gateway;

        public PostNewNoteUseCase(INotesGateway gateway)
        {
            _gateway = gateway;
        }

        [LogCall]
        public async Task<NoteResponseObject> ExecuteAsync(CreateNoteRequest createNoteRequest)
        {
            var result = await _gateway.PostNewNoteAsync(createNoteRequest).ConfigureAwait(false);
            return NoteResponseObject.Create(result);
        }
    }
}
