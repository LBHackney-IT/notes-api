using System.Text.Json.Serialization;

namespace NotesApi.V1.Domain
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum TargetType
    {
        Person,
        Asset,
        Tenure,
        Repair
    }

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum Tag
    {
        Person,
        Update,
        Complaint
    }
}
