using System.Collections.Generic;
using System.Linq;
using NotesApi.V2.Boundary.Response;
using NotesApi.V2.Domain;

namespace NotesApi.V2.Factories
{
    public static class ResponseFactory
    {
        public static NoteResponseObject ToResponse(this Note domain)
        {
            return new NoteResponseObject
            {
                Author = domain.Author,
                Categorisation = domain.Categorisation,
                CreatedAt = domain.CreatedAt,
                Description = domain.Description,
                Id = domain.Id,
                TargetId = domain.TargetId,
                TargetType = domain.TargetType,
                Title = domain.Title,
                Highlight = domain.Highlight
            };
        }

        public static List<NoteResponseObject> ToResponse(this IEnumerable<Note> domainList)
        {
            if (null == domainList) return new List<NoteResponseObject>();

            return domainList.Select(domain => domain.ToResponse()).ToList();
        }
    }
}
