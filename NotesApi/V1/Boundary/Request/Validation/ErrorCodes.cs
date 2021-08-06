namespace NotesApi.V1.Boundary.Request.Validation
{
    public static class ErrorCodes
    {
        public const string DescriptionMandatory = "W2";
        public const string DescriptionTooLong = "W3";
        public const string InvalidEmail = "W100";
        public const string XssCheckFailure = "W666";
    }
}
