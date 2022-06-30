using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using Hackney.Core.DynamoDb;
using Hackney.Core.Sns;
using Hackney.Core.Testing.Sns;
using Newtonsoft.Json;
using NotesApi.Tests.V2.E2ETests.Fixtures;
using NotesApi.Tests.V2.E2ETests.Steps.Constants;
using NotesApi.V2.Domain;
using NotesApi.V2.Boundary.Request;
using NotesApi.V2.Boundary.Response;
using NotesApi.V2.Infrastructure;
using NotesApi.V2.Infrastructure.JWT;
using JsonSerializer = System.Text.Json.JsonSerializer;
using NotesApi.V2.Factories;

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

            var message = new HttpRequestMessage(HttpMethod.Post, uri);
            message.Content = new StringContent(json, Encoding.UTF8, "application/json");
            message.Method = HttpMethod.Post;

            var token = TestToken.Value;
            message.Headers.Add("Authorization", token);

            _lastResponse = await _httpClient.SendAsync(message).ConfigureAwait(false);
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

        public async Task ThenTheNoteCreatedEventIsRaised(NotesFixture notesFixture, ISnsFixture snsFixture, string sourceDomain)
        {
            var dbRecord = notesFixture.Notes.LastOrDefault();

            Action<EntityEventSns> verifyFunc = (actual) =>
            {
                actual.CorrelationId.Should().NotBeEmpty();
                actual.DateTime.Should().BeCloseTo(DateTime.UtcNow, 2000);
                actual.EntityId.Should().Be(dbRecord.TargetId);

                var actualNewData = JsonConvert.DeserializeObject<Note>(actual.EventData.NewData.ToString());
                actualNewData.Should().BeEquivalentTo(dbRecord.ToDomain());

                actual.EventData.OldData.Should().BeNull();

                actual.EventType.Should().Be(NoteCreatedEventConstants.EVENTTYPE);
                actual.Id.Should().NotBeEmpty();
                actual.SourceDomain.Should().Be(sourceDomain);
                actual.SourceSystem.Should().Be(NoteCreatedEventConstants.SOURCE_SYSTEM);
                actual.User.Email.Should().Be(TestToken.UserEmail);
                actual.User.Name.Should().Be(TestToken.UserName);
                actual.Version.Should().Be(NoteCreatedEventConstants.V2_VERSION);
            };

            var snsVerifier = snsFixture.GetSnsEventVerifier<EntityEventSns>();
            var snsResult = await snsVerifier.VerifySnsEventRaised(verifyFunc);

            if (!snsResult && snsVerifier.LastException != null) throw snsVerifier.LastException;
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
