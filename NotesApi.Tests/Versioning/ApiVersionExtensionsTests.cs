using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NotesApi.Versioning;
using Xunit;

namespace NotesApi.Tests.Versioning
{
    public class ApiVersionExtensionsTests
    {
        [Fact]
        public void GetFormattedApiVersionTest()
        {
            var version = new ApiVersion(1, 1);
            version.GetFormattedApiVersion().Should().Be($"v{version.ToString()}");
        }
    }
}
