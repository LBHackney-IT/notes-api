using System.Threading.Tasks;
using Hackney.Core.Logging;
using NotesApi.V2.Boundary.Request;
using NotesApi.V2.Boundary.Response;
using NotesApi.V2.Gateways;
using NotesApi.V2.UseCase.Interfaces;
using NotesApi.V2.Factories;
using System;
using Hackney.Core.Sns;
using Hackney.Core.JWT;

namespace NotesApi.V2.UseCase
{
    public class PostNewNoteUseCase : IPostNewNoteUseCase
    {
        private readonly INotesGateway _gateway;
        private readonly ISnsFactory _snsFactory;
        private readonly ISnsGateway _snsGateway;

        public PostNewNoteUseCase(INotesGateway gateway, ISnsFactory snsFactory, ISnsGateway snsGateway)
        {
            _gateway = gateway;
            _snsFactory = snsFactory;
            _snsGateway = snsGateway;
        }

        [LogCall]
        public async Task<NoteResponseObject> ExecuteAsync(CreateNoteRequest createNoteRequest, Token token)
        {
            var result = await _gateway.PostNewNoteAsync(createNoteRequest).ConfigureAwait(false);

            var snsMessage = _snsFactory.NoteCreated(result, token);
            var topicArn = Environment.GetEnvironmentVariable("NOTES_SNS_ARN");
            await _snsGateway.Publish(snsMessage, topicArn).ConfigureAwait(false);

            return NoteResponseObject.Create(result);
        }
    }
}
