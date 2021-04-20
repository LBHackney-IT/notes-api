using Microsoft.AspNetCore.Mvc;
using System;
using System.ComponentModel.DataAnnotations;

namespace NotesApi.V1.Domain.Queries
{
    public class GetNotesByTargetIdQuery
    {
        [Required]
        [FromQuery]
        public Guid TargetId { get; set; }

        [FromQuery]
        public string PaginationToken { get; set; }

        [FromQuery]
        public int? PageSize { get; set; }
    }
}
