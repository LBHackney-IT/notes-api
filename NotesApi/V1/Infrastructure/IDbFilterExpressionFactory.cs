using System.Collections.Generic;
using NotesApi.V1.Gateways;

namespace NotesApi.V1.Infrastructure
{
    public interface IDbFilterExpressionFactory
    {
        string Create(List<ExcludedCategory> categories);
    }
}
