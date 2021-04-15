using System.Text.Json.Serialization;

namespace NotesApi.V1.Domain
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum TargetType
    {
        person,
        asset,
        tenure,
        repair
    }

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum Tag
    {
        person,
        update,
        complaint
    }
}
