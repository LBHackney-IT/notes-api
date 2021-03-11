using System.Collections.Generic;
using NotesApi.V1.Domain;
using NotesApi.V1.Factories;
using NotesApi.V1.Infrastructure;

namespace NotesApi.V1.Gateways
{
    public class NotesApiGateway : INotesApiGateway
    {
        private readonly DatabaseContext _databaseContext;

        public NotesApiGateway(DatabaseContext databaseContext)
        {
            _databaseContext = databaseContext;
        }

        public Entity GetEntityById(int id)
        {
            var result = _databaseContext.DatabaseEntities.Find(id);

            return result?.ToDomain();
        }

        public List<Entity> GetAll()
        {
            return new List<Entity>();
        }
    }
}
