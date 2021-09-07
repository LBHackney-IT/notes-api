using System.Collections.Generic;
using NotesApi.V1.Domain;

namespace NotesApi.V1.Infrastructure
{
    public interface INotesDBFilter
    {
        List<Note> Filter(List<Note> notes);
    }
}
