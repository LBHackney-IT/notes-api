using NotesApi.V1.Boundary.Response;

namespace NotesApi.V1.UseCase.Interfaces
{
    public interface IGetByIdUseCase
    {
        ResponseObject Execute(int id);
    }
}
