using System.Collections.Generic;

namespace NotesApi.V1.Infrastructure
{
    public interface IDbFilterExpressionFactory
    {
        string Create(List<ExcludedCategory> categories);
    }
}
