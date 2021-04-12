using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NotesApi.V1.Domain;

namespace NotesApi.V1.Gateways
{
    public interface INotesApiGateway
    {
        Task<List<Note>> GetByTargetIdAsync(Guid targetId);
    }
}
