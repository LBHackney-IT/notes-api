using NotesApi.V1.Boundary.Response;
using NotesApi.V1.Domain.Queries;
using NotesApi.V1.Factories;
using NotesApi.V1.Gateways;
using NotesApi.V1.UseCase.Interfaces;
using System.Threading.Tasks;

namespace NotesApi.V1.UseCase
{
    public class GetByTargetIdUseCase : IGetByTargetIdUseCase
    {
        private readonly INotesGateway _gateway;

        public GetByTargetIdUseCase(INotesGateway gateway)
        {
            _gateway = gateway;
        }

        public async Task<PagedResult<NoteResponseObject>> ExecuteAsync(GetNotesByTargetIdQuery query)
        {
            var gatewayResult = await _gateway.GetByTargetIdAsync(query).ConfigureAwait(false);
            return new PagedResult<NoteResponseObject>(gatewayResult.Results.ToResponse(), gatewayResult.PaginationDetails);
        }
    }
}
