using System.Collections.Generic;
using System.Text;
using NotesApi.V1.Gateways;

namespace NotesApi.V1.Infrastructure
{
    public class DbFilterExpressionFactory : IDbFilterExpressionFactory
    {
        public string Create(List<ExcludedCategory> categories)
        {
            StringBuilder filterExpression = new StringBuilder();

            foreach (var category in categories)
            {
                filterExpression.Append($"#categorisation.#category <> {category.CategoryValueKey} and ");
            }

            filterExpression = filterExpression.Remove((filterExpression.Length - 5), 5);
            return filterExpression.ToString();
        }
    }
}
