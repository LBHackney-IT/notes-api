using NotesApi.V1.Boundary.Response;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NotesApi.V1.UseCase.Interfaces
{
    public interface IGetByTargetIdUseCase
    {
        Task<List<NoteResponseObject>> ExecuteAsync(Guid targetId);
    }
}
