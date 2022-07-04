using System.Text.Json.Serialization;

namespace NotesApi.V2.Domain
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum TargetType
    {
        person,
        asset,
        tenure,
        repair,
        process
    }
}
