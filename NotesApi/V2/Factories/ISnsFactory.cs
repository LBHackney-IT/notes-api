using Hackney.Core.JWT;
using Hackney.Core.Sns;
using NotesApi.V2.Domain;

namespace NotesApi.V2.Factories
{
    public interface ISnsFactory
    {
        EntityEventSns NoteCreated(Note note, Token token);
    }
}
