using System.Collections.Generic;

namespace NotesApi.V1
{
    public class PagedResult<T> where T : class
    {
        public List<T> Results { get; set; } = new List<T>();

        private string _paginationToken;
        public string PaginationToken
        {
            get { return _paginationToken; }
            set { _paginationToken = ValidatePaginationToken(value); }
        }

        public PagedResult() { }
        public PagedResult(IEnumerable<T> results, string paginationToken)
        {
            if (null != results) Results.AddRange(results);
            PaginationToken = paginationToken;
        }

        private static string ValidatePaginationToken(string paginationToken)
        {
            // The AWS SDK can either return an empty JSON object (i.e. '{}') when there are no more results.
            if (string.IsNullOrWhiteSpace(paginationToken?.Trim(' ', '{', '}')))
                return null;

            // Or a JSON object with escaped double quotes (i.e. '\"')
            return paginationToken.Replace("\"", "'");
        }
    }
}
