using System.Collections.Generic;
using NotesApi.V1.Gateways;

namespace NotesApi.V1.Infrastructure
{
    public interface IExcludedCategoriesFactory
    {
        List<ExcludedCategory> Create(List<string> categoryValues);
    }
}
