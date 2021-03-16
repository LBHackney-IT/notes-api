using NotesApi.V1.Boundary.Response;
using NotesApi.V1.Factories;
using NotesApi.V1.Gateways;
using NotesApi.V1.UseCase.Interfaces;

namespace NotesApi.V1.UseCase
{
    //TODO: Rename class name and interface name to reflect the entity they are representing eg. GetAllClaimantsUseCase
    public class GetAllUseCase : IGetAllUseCase
    {
        private readonly INotesApiGateway _gateway;
        public GetAllUseCase(INotesApiGateway gateway)
        {
            _gateway = gateway;
        }

        public ResponseObjectList Execute()
        {
            return new ResponseObjectList { ResponseObjects = _gateway.GetAll().ToResponse() };
        }
    }
}
