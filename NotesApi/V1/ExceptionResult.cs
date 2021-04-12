using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace NotesApi.V1
{
    // TODO: This should go in a common NuGet package...
    // TODO: Ideally this should use System.Text.Json and not Newtonsoft,
    // but the [JsonConstructor] attribute is currently only available for .NET v.5 (and not .net core v.3.1)

    public class ExceptionResult
    {
        public const int DefaultStatusCode = 500;

        private static JsonSerializerSettings _settings = new JsonSerializerSettings()
        {
            NullValueHandling = NullValueHandling.Ignore,
            ContractResolver = new CamelCasePropertyNamesContractResolver()
        };

        public ExceptionResult(string message, string traceId, string correlationId, int statusCode = DefaultStatusCode)
        {
            Message = message;
            TraceId = traceId;
            CorrelationId = correlationId;
            StatusCode = statusCode;
        }

        public string Message { get; private set; }
        public string TraceId { get; private set; }
        public string CorrelationId { get; private set; }
        public int StatusCode { get; private set; }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this, _settings);
        }
    }
}
