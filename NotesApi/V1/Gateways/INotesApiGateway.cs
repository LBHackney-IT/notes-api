using System.Collections.Generic;
using NotesApi.V1.Domain;

namespace NotesApi.V1.Gateways
{
    public interface INotesApiGateway
    {
        Entity GetEntityById(int id);

        List<Entity> GetAll();
    }
}
