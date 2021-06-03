using Microsoft.AspNetCore.Mvc;
using System;

namespace NotesApi.V1.Boundary.Request
{
    public class GetNotesByTargetIdQuery
    {
        [FromQuery]
        public Guid? TargetId { get; set; }

        [FromQuery]
        public string PaginationToken { get; set; }

        [FromQuery]
        public int? PageSize { get; set; }
    }
}
