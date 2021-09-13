using System;
using Microsoft.AspNetCore.Mvc;

namespace NotesApi.V2.Boundary.Request
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
