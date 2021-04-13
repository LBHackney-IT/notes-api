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

        public async Task WhenTheTargetNotesAreRequested(string id)
        {
            var uri = new Uri($"api/v1/notes?targetId={id}", UriKind.Relative);
            _lastResponse = await _httpClient.GetAsync(uri).ConfigureAwait(false);
        }

        public async Task ThenTheTargetNotesAreReturned(List<NoteDb> expectedNotes)
        {
            _lastResponse.StatusCode.Should().Be(HttpStatusCode.OK);

            var responseContent = await _lastResponse.Content.ReadAsStringAsync().ConfigureAwait(false);
            var apiResult = JsonSerializer.Deserialize<PagedResult<NoteResponseObject>>(responseContent, CreateJsonOptions());

            apiResult.Results.Should().BeEquivalentTo(expectedNotes);

            IsDateTimeListInDescendingOrder(apiResult.Results.Select(x => x.DateTime)).Should().BeTrue();
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
