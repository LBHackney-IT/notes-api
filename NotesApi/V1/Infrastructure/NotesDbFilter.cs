using System;
using System.Collections.Generic;
using System.Linq;
using NotesApi.V1.Domain;

namespace NotesApi.V1.Infrastructure
{
    public class NotesDbFilter : INotesDBFilter
    {
        public List<Note> Filter(List<Note> notes)
        {
            return notes.Where(x =>
                !string.Equals(x.Categorisation.Category, "ASB", StringComparison.CurrentCultureIgnoreCase)).ToList();
        }
    }
}
