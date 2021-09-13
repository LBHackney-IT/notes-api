using System.Threading.Tasks;
using Hackney.Core.DynamoDb;
using Hackney.Core.Logging;
using NotesApi.V2.Boundary.Request;
using NotesApi.V2.Boundary.Response;
using NotesApi.V2.Factories;
using NotesApi.V2.Gateways;
using NotesApi.V2.UseCase.Interfaces;

namespace NotesApi.V2.UseCase
{
    public class GetByTargetIdUseCase : IGetByTargetIdUseCase
    {
        private readonly INotesGateway _gateway;

        public GetByTargetIdUseCase(INotesGateway gateway)
        {
            _gateway = gateway;
        }

        [LogCall]
        public async Task<PagedResult<NoteResponseObject>> ExecuteAsync(GetNotesByTargetIdQuery query)
        {
            var gatewayResult = await _gateway.GetByTargetIdAsync(query).ConfigureAwait(false);
            return new PagedResult<NoteResponseObject>(gatewayResult.Results.ToResponse(), gatewayResult.PaginationDetails);
        }
    }
}
