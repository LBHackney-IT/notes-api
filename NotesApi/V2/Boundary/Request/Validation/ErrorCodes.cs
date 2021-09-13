namespace NotesApi.V2.Boundary.Request.Validation
{
    public static class ErrorCodes
    {
        public const string DescriptionMandatory = "W2";
        public const string DescriptionTooLong = "W3";
        public const string InvalidEmail = "W40";
        public const string XssCheckFailure = "W42";
    }
}
