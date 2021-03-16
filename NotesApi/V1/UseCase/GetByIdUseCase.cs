using NotesApi.V1.Boundary.Response;
using NotesApi.V1.Factories;
using NotesApi.V1.Gateways;
using NotesApi.V1.UseCase.Interfaces;

namespace NotesApi.V1.UseCase
{
    //TODO: Rename class name and interface name to reflect the entity they are representing eg. GetClaimantByIdUseCase
    public class GetByIdUseCase : IGetByIdUseCase
    {
        private INotesApiGateway _gateway;
        public GetByIdUseCase(INotesApiGateway gateway)
        {
            _gateway = gateway;
        }

        //TODO: rename id to the name of the identifier that will be used for this API, the type may also need to change
        public ResponseObject Execute(int id)
        {
            return _gateway.GetEntityById(id).ToResponse();
        }
    }
}
