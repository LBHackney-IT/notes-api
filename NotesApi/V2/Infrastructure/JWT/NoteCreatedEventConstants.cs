namespace NotesApi.V2.Infrastructure.JWT
{
    public class NoteCreatedEventConstants
    {
        public const string V2_VERSION = "V2";
        public const string SOURCE_DOMAIN = "Notes";
        public const string SOURCE_SYSTEM = "NotesAPI";

        public const string PERSON_NOTE_EVENT = "NoteCreatedAgainstPersonEvent";
        public const string ASSET_NOTE_EVENT = "NoteCreatedAgainstAssetEvent";
        public const string TENURE_NOTE_EVENT = "NoteCreatedAgainstTenureEvent";
        public const string REPAIR_NOTE_EVENT = "NoteCreatedAgainstRepairEvent";
        public const string PROCESS_NOTE_EVENT = "NoteCreatedAgainstProcessEvent";

    }
}
