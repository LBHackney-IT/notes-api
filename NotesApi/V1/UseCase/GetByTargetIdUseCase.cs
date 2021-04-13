using NotesApi.V1.Boundary.Response;
using NotesApi.V1.Factories;
using NotesApi.V1.Gateways;
using NotesApi.V1.UseCase.Interfaces;
using System;
using System.Collections.Generic;
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

        public async Task<List<NoteResponseObject>> ExecuteAsync(Guid targetId)
        {
            return (await _gateway.GetByTargetIdAsync(targetId).ConfigureAwait(false)).ToResponse();
        }
    }
}
