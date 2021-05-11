using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using NotesApi.Versioning;
using Xunit;

namespace NotesApi.Tests.Versioning
{
    public class ApiVersionDescriptionExtensionsTests
    {
        [Fact]
        public void GetFormattedApiVersionTest()
        {
            var version = new ApiVersion(1, 1);
            ApiVersionDescription sut = new ApiVersionDescription(version, null, false);
            sut.GetFormattedApiVersion().Should().Be($"v{version.ToString()}");
        }
    }
}
