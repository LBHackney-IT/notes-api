using System;
using System.Collections.Generic;
using System.Linq;
using NotesApi.V1.Gateways;

namespace NotesApi.V1.Infrastructure
{
    public class ExcludedCategoriesFactory : IExcludedCategoriesFactory
    {
        public List<ExcludedCategory> Create(List<string> categoryValues)
        {
            var excludedCategoriesList = new List<ExcludedCategory>();

            foreach (string categoryValue in categoryValues)
            {
                var excludedCategory = new ExcludedCategory { CategoryValue = categoryValue };

                var categoryValueKey = GenerateValueKey(excludedCategoriesList);

                excludedCategory.CategoryValueKey = categoryValueKey;
                excludedCategoriesList.Add(excludedCategory);
            }

            return excludedCategoriesList;
        }

        private static string GenerateValueKey(List<ExcludedCategory> excludedCategoriesList)
        {
            var categoryValueKey = GenerateRandomParameterName(":");

            while (excludedCategoriesList.Any(x => x.CategoryValueKey == categoryValueKey))
            {
                categoryValueKey = GenerateRandomParameterName(":");
            }

            return categoryValueKey;
        }

        private static string GenerateRandomParameterName(string prefix)
        {
            var chars = "abdefghijklmnopqrstuvwxyz";
            var stringChars = new char[8];
            var random = new Random();

            for (int i = 0; i < stringChars.Length; i++)
            {
                stringChars[i] = chars[random.Next(chars.Length)];
            }

            return prefix + new string(stringChars);
        }
    }
}
