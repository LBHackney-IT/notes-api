using FluentAssertions;
using NotesApi.V1;
using NotesApi.V1.Boundary.Response;
using NotesApi.V1.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace NotesApi.Tests.V1.E2ETests.Steps
{
    public class GetNotesSteps
    {
        private readonly HttpClient _httpClient;

        private HttpResponseMessage _lastResponse;
        private readonly List<NoteResponseObject> _pagedNotes = new List<NoteResponseObject>();
        private static readonly JsonSerializerOptions _jsonOptions = CreateJsonOptions();

        public GetNotesSteps(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        private static JsonSerializerOptions CreateJsonOptions()
        {
            var options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                WriteIndented = true
            };
            options.Converters.Add(new JsonStringEnumConverter());
            return options;
        }

        private static bool IsDateTimeListInDescendingOrder(IEnumerable<DateTime> dateTimeList)
        {
            var previousDateTimeItem = dateTimeList.FirstOrDefault();
            foreach (DateTime currentDateTimeItem in dateTimeList)
            {
                if (currentDateTimeItem.CompareTo(previousDateTimeItem) > 0)
                    return false;
            }

            return true;
        }

        private async Task<HttpResponseMessage> CallApi(string id, string paginationToken = null, int? pageSize = null)
        {
            var route = $"api/v1/notes?targetId={id}";
            if (!string.IsNullOrEmpty(paginationToken))
                route += $"&paginationToken={paginationToken}";
            if (pageSize.HasValue)
                route += $"&pageSize={pageSize.Value}";
            var uri = new Uri(route, UriKind.Relative);
            return await _httpClient.GetAsync(uri).ConfigureAwait(false);
        }

        private static async Task<PagedResult<NoteResponseObject>> ExtractResultFromHttpResponse(HttpResponseMessage response)
        {
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var responseContent = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            var apiResult = JsonSerializer.Deserialize<PagedResult<NoteResponseObject>>(responseContent, _jsonOptions);
            return apiResult;
        }

        public async Task WhenTheTargetNotesAreRequested(string id)
        {
            _lastResponse = await CallApi(id).ConfigureAwait(false);
        }

        public async Task WhenTheTargetNotesAreRequestedWithPageSize(string id, int? pageSize = null)
        {
            _lastResponse = await CallApi(id, null, pageSize).ConfigureAwait(false);
        }

        public async Task WhenAllTheTargetNotesAreRequested(string id)
        {
            string pageToken = null;
            do
            {
                var response = await CallApi(id, pageToken).ConfigureAwait(false);
                var apiResult = await ExtractResultFromHttpResponse(response).ConfigureAwait(false);
                _pagedNotes.AddRange(apiResult.Results);

                pageToken = apiResult.PaginationDetails.NextToken;
            }
            while (!string.IsNullOrEmpty(pageToken));
        }

        public async Task ThenTheTargetNotesAreReturned(List<NoteDb> expectedNotes)
        {
            var apiResult = await ExtractResultFromHttpResponse(_lastResponse).ConfigureAwait(false);
            apiResult.Results.Should().BeEquivalentTo(expectedNotes);
            IsDateTimeListInDescendingOrder(apiResult.Results.Select(x => x.DateTime)).Should().BeTrue();
        }

        public async Task ThenTheTargetNotesAreReturnedByPageSize(List<NoteDb> expectedNotes, int? pageSize)
        {
            var expectedPageSize = 10;
            if (pageSize.HasValue)
                expectedPageSize = (pageSize.Value > expectedNotes.Count) ? expectedNotes.Count : pageSize.Value;

            var apiResult = await ExtractResultFromHttpResponse(_lastResponse).ConfigureAwait(false);
            apiResult.Results.Count.Should().Be(expectedPageSize);
            apiResult.Results.Should().BeEquivalentTo(expectedNotes.OrderByDescending(x => x.DateTime).Take(expectedPageSize));

            IsDateTimeListInDescendingOrder(apiResult.Results.Select(x => x.DateTime)).Should().BeTrue();
        }

        public async Task ThenTheFirstPageOfTargetNotesAreReturned(List<NoteDb> expectedNotes)
        {
            var apiResult = await ExtractResultFromHttpResponse(_lastResponse).ConfigureAwait(false);
            apiResult.PaginationDetails.NextToken.Should().NotBeNullOrEmpty();
            apiResult.Results.Count.Should().Be(10);
            apiResult.Results.Should().BeEquivalentTo(expectedNotes.OrderByDescending(x => x.DateTime).Take(10));

            IsDateTimeListInDescendingOrder(apiResult.Results.Select(x => x.DateTime)).Should().BeTrue();
        }

        public void ThenAllTheTargetNotesAreReturned(List<NoteDb> expectedNotes)
        {
            _pagedNotes.Should().BeEquivalentTo(expectedNotes.OrderByDescending(x => x.DateTime));
            IsDateTimeListInDescendingOrder(_pagedNotes.Select(x => x.DateTime)).Should().BeTrue();
        }

        public void ThenBadRequestIsReturned()
        {
            _lastResponse.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        public void ThenNotFoundIsReturned()
        {
            _lastResponse.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }
    }
}
