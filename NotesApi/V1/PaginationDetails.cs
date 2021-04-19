using Microsoft.IdentityModel.Tokens;
using System.Text.Json.Serialization;

namespace NotesApi.V1
{
    public class PaginationDetails
    {
        [JsonIgnore]
        public bool HasNext => !string.IsNullOrEmpty(NextToken);
        public string NextToken { get; set; }

        [JsonIgnore]
        public bool HasPrevious => !string.IsNullOrEmpty(PreviousToken);
        public string PreviousToken { get; set; }

        public PaginationDetails() { }
        public PaginationDetails(string rawNextToken, string rawPreviousToken)
        {
            EncodeNextToken(rawNextToken);
            EncodePreviousToken(rawPreviousToken);
        }

        public static string EncodeToken(string rawToken)
        {
            // The AWS SDK can either return an empty JSON object (i.e. '{}') when there are no more results.
            if (string.IsNullOrWhiteSpace(rawToken?.Trim(' ', '{', '}')))
                return null;

            return Base64UrlEncoder.Encode(rawToken);
        }

        public static string DecodeToken(string token)
        {
            if (string.IsNullOrWhiteSpace(token))
                return null;

            return Base64UrlEncoder.Decode(token);
        }

        public void EncodeNextToken(string rawToken)
        {
            NextToken = EncodeToken(rawToken);
        }

        public string DecodeNextToken()
        {
            return DecodeToken(NextToken);
        }

        public void EncodePreviousToken(string rawToken)
        {
            PreviousToken = EncodeToken(rawToken);
        }

        public string DecodePreviousToken()
        {
            return DecodeToken(PreviousToken);
        }
    }
}
