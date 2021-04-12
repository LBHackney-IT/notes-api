using NotesApi.V1.Boundary.Response;
using NotesApi.V1.UseCase.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;

namespace NotesApi.V1.Controllers
{
    [ApiController]
    [Route("api/v1/notes")]
    [Produces("application/json")]
    [ApiVersion("1.0")]
    public class NotesApiController : BaseController
    {
        private readonly IGetByTargetIdUseCase _getByTargetIdUseCase;
        public NotesApiController(IGetByTargetIdUseCase getByTargetIdUseCase)
        {
            _getByTargetIdUseCase = getByTargetIdUseCase;
        }

        //TO DO: add xml comments containing information that will be included in the auto generated swagger docs (https://github.com/LBHackney-IT/lbh-base-api/wiki/Controllers-and-Response-Objects)
        /// <summary>
        /// Retrieves all notes for the supplied targetId
        /// </summary>
        /// <response code="200">Returns the list of notes for the supplied target id.</response>
        /// <response code="400">Invalid Query Parameter.</response>
        /// <response code="404">No notes found for the supplied targetId</response>
        [ProducesResponseType(typeof(List<NoteResponseObject>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpGet]
        public async Task<IActionResult> GetByTargetIdAsync([FromQuery] Guid targetId)
        {
            var notes = await _getByTargetIdUseCase.ExecuteAsync(targetId).ConfigureAwait(false);
            if ((null == notes) || !notes.Any()) return NotFound(targetId);

            return Ok(notes);
        }

    }
}
