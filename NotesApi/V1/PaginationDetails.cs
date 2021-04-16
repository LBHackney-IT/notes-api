using Microsoft.IdentityModel.Tokens;
using System.Text.Json.Serialization;

namespace NotesApi.V1
{
    public class PaginationDetails
    {
        [JsonIgnore]
        public bool HasMore => !string.IsNullOrEmpty(MoreToken);
        public string MoreToken { get; set; }

        public PaginationDetails() { }
        public PaginationDetails(string rawMoreToken)
        {
            EncodeMoreToken(rawMoreToken);
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

        public void EncodeMoreToken(string rawToken)
        {
            MoreToken = EncodeToken(rawToken);
        }

        public string DecodeMoreToken()
        {
            return DecodeToken(MoreToken);
        }
    }
}
