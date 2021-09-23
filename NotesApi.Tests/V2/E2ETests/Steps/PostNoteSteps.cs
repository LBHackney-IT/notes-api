using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using Hackney.Core.DynamoDb;
using Newtonsoft.Json;
using NotesApi.Tests.V2.E2ETests.Fixtures;
using NotesApi.V2.Boundary.Request;
using NotesApi.V2.Boundary.Response;
using NotesApi.V2.Infrastructure;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace NotesApi.Tests.V2.E2ETests.Steps
{
    public class PostNoteSteps : BaseSteps
    {
        public PostNoteSteps(HttpClient httpClient) : base(httpClient)
        { }

        private async Task<HttpResponseMessage> PostToApi(CreateNoteRequest request)
        {
            var json = JsonConvert.SerializeObject(request);
            return await PostJsonToApi(json).ConfigureAwait(false);
        }

        private async Task<HttpResponseMessage> PostJsonToApi(string json)
        {
            var route = $"api/v2/notes";
            var uri = new Uri(route, UriKind.Relative);

            var data = new StringContent(json, Encoding.UTF8, "application/json");

            _lastResponse = await _httpClient.PostAsync(uri, data).ConfigureAwait(false);
            return _lastResponse;
        }

        private async Task<HttpResponseMessage> CallApi(string id, string paginationToken = null, int? pageSize = null)
        {
            var route = $"api/v2/notes?targetId={id}";
            if (!string.IsNullOrEmpty(paginationToken))
                route += $"&paginationToken={paginationToken}";
            if (pageSize.HasValue)
                route += $"&pageSize={pageSize.Value}";
            var uri = new Uri(route, UriKind.Relative);
            return await _httpClient.GetAsync(uri).ConfigureAwait(false);
        }

        private async Task<PagedResult<NoteResponseObject>> ExtractResultFromHttpResponse(HttpResponseMessage response)
        {
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var responseContent = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            var apiResult = JsonSerializer.Deserialize<PagedResult<NoteResponseObject>>(responseContent, _jsonOptions);
            return apiResult;
        }

        #region When

        public async Task WhenPostingANote(NotesFixture notesFixture)
        {
            var result = await PostToApi(notesFixture.NoteRequest).ConfigureAwait(false);
            notesFixture.NoteResponse =
                JsonConvert.DeserializeObject<NoteResponseObject>(result.Content.ReadAsStringAsync().Result);
        }

        public async Task WhenPostingTheInvalidPayload(NotesFixture notesFixture)
        {
            var result = await PostJsonToApi(notesFixture.InvalidPayload).ConfigureAwait(false);
            notesFixture.NoteResponse =
                JsonConvert.DeserializeObject<NoteResponseObject>(result.Content.ReadAsStringAsync().Result);
        }

        #endregion When

        #region Then

        public async Task ThenTheNoteHasBeenPersisted(NotesFixture notesFixture)
        {
            var responseObject = notesFixture.NoteResponse;
            var response = await CallApi(responseObject.TargetId.ToString()).ConfigureAwait(false);
            var apiResult = await ExtractResultFromHttpResponse(response).ConfigureAwait(false);

            var note = apiResult.Results.FirstOrDefault(x => x.TargetId == responseObject.TargetId && x.Id == responseObject.Id);
            note.Should().NotBeNull();
            note.Id.Should().Be(responseObject.Id);

            // Remove after use...
            var dbNote = new NoteDb()
            {
                Author = note.Author,
                Title = note.Title,
                Categorisation = note.Categorisation,
                CreatedAt = note.CreatedAt,
                Description = note.Description,
                Id = note.Id,
                TargetId = note.TargetId,
                TargetType = note.TargetType,
                Highlight = note.Highlight
            };
            notesFixture.Notes.Add(dbNote);
        }

        public void ThenBadRequestIsReturned()
        {
            _lastResponse.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        public async Task ThenBadRequestValidationErrorResultIsReturned(string propertyName)
        {
            await ThenBadRequestValidationErrorResultIsReturned(propertyName, null, null).ConfigureAwait(false);
        }

        public async Task ThenBadRequestValidationErrorResultIsReturned(string propertyName, string errorCode)
        {
            await ThenBadRequestValidationErrorResultIsReturned(propertyName, errorCode, null).ConfigureAwait(false);
        }
        public async Task ThenBadRequestValidationErrorResultIsReturned(string propertyName, string errorCode, string errorMsg)
        {
            _lastResponse.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            var resultBody = await _lastResponse.Content.ReadAsStringAsync().ConfigureAwait(false);
            resultBody.Should().Contain("One or more validation errors occurred");
            resultBody.Should().Contain(propertyName);
            if (null != errorCode)
                resultBody.Should().Contain(errorCode);
            if (null != errorMsg)
                resultBody.Should().Contain(errorMsg);
        }

        #endregion Then
    }
}
