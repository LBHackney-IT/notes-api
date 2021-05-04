using System;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NotesApi.V1.Boundary.Response;
using NotesApi.V1.Domain.Queries;
using NotesApi.V1.Logging;
using NotesApi.V1.UseCase.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NotesApi.V1.Boundary;

namespace NotesApi.V1.Controllers
{
    [ApiController]
    [Route("api/v1/notes")]
    [Produces("application/json")]
    [ApiVersion("1.0")]
    public class NotesApiController : BaseController
    {
        private readonly IGetByTargetIdUseCase _getByTargetIdUseCase;
        private readonly IPostNewNoteUseCase _newNoteUseCase;

        public NotesApiController(IGetByTargetIdUseCase getByTargetIdUseCase, IPostNewNoteUseCase newNoteUseCase)
        {
            _getByTargetIdUseCase = getByTargetIdUseCase;
            _newNoteUseCase = newNoteUseCase;
        }

        /// <summary>
        /// Retrieves all notes for the supplied targetId.
        /// If a pagination token is provided (returned from a previous call where the results set > 10)
        /// then the query will continue from the last record returned in the preivious query.
        /// </summary>
        /// <response code="200">Returns the list of notes for the supplied target id.</response>
        /// <response code="400">Invalid Query Parameter.</response>
        /// <response code="404">No notes found for the supplied targetId</response>
        [ProducesResponseType(typeof(List<NoteResponseObject>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpGet]
        [LogCall(LogLevel.Information)]
        public async Task<IActionResult> GetByTargetIdAsync([FromQuery] GetNotesByTargetIdQuery query)
        {
            var response = await _getByTargetIdUseCase.ExecuteAsync(query).ConfigureAwait(false);
            if ((null == response) || !response.Results.Any()) return NotFound(query.TargetId);

            return Ok(response);
        }

        /// <summary>
        /// Creates a new note entry
        /// </summary>
        /// <response code="201">Returns the note created with its ID</response>
        /// <response code="400">Invalid fields in the post parameter.</response>
        /// <response code="500">Internal server error</response>
        [ProducesResponseType(typeof(List<NoteResponseObject>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpPost]
        public async Task<IActionResult> PostNewNote([FromBody] CreateNoteRequest noteRequest)
        {
            var newNote = await _newNoteUseCase.ExecuteAsync(noteRequest).ConfigureAwait(false);
            return Created(new Uri($"api/v1/notes?targetId={newNote.TargetId}", UriKind.Relative), newNote);
        }
    }
}
