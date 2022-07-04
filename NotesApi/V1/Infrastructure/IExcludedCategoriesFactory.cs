using System.Collections.Generic;

namespace NotesApi.V1.Infrastructure
{
    public interface IExcludedCategoriesFactory
    {
        List<ExcludedCategory> Create(List<string> categoryValues);
    }
}
