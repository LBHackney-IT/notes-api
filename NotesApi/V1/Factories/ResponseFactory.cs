using System.Collections.Generic;
using System.Linq;
using NotesApi.V1.Boundary.Response;
using NotesApi.V1.Domain;

namespace NotesApi.V1.Factories
{
    public static class ResponseFactory
    {
        public static NoteResponseObject ToResponse(this Note domain)
        {
            return new NoteResponseObject
            {
                Author = domain.Author,
                Categorisation = domain.Categorisation,
                DateTime = domain.DateTime,
                Description = domain.Description,
                Id = domain.Id,
                Tags = domain.Tags,
                TargetId = domain.TargetId,
                TargetType = domain.TargetType
            };
        }

        public static List<NoteResponseObject> ToResponse(this IEnumerable<Note> domainList)
        {
            if (null == domainList) return new List<NoteResponseObject>();

            return domainList.Select(domain => domain.ToResponse()).ToList();
        }
    }
}
