using System.Threading.Tasks;
using Hackney.Core.Logging;
using NotesApi.V1.Boundary.Response;
using NotesApi.V1.Domain.Queries;
using NotesApi.V1.Gateways;
using NotesApi.V1.UseCase.Interfaces;

namespace NotesApi.V1.UseCase
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
