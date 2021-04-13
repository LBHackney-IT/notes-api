using FluentAssertions;
using Microsoft.AspNetCore.Http;
using NotesApi.V1;
using System.Threading.Tasks;
using Xunit;

namespace NotesApi.Tests.V1
{
    public class CorrelationMiddlewareTest
    {
        private readonly CorrelationMiddleware _sut;

        public CorrelationMiddlewareTest()
        {
            _sut = new CorrelationMiddleware(null);
        }

        [Fact]
        public async Task DoesNotReplaceCorrelationIdIfOneExists()
        {
            // Arrange
            var httpContext = new DefaultHttpContext();
            var headerValue = "123";

            httpContext.HttpContext.Request.Headers.Add(CorrelationConstants.CorrelationId, headerValue);

            // Act
            await _sut.InvokeAsync(httpContext).ConfigureAwait(false);

            // Assert
            httpContext.HttpContext.Request.Headers[CorrelationConstants.CorrelationId].Should().BeEquivalentTo(headerValue);
        }

        [Fact]
        public async Task AddsCorrelationIdIfOneDoesNotExist()
        {
            // Arrange
            var httpContext = new DefaultHttpContext();

            // Act
            await _sut.InvokeAsync(httpContext).ConfigureAwait(false);

            // Assert
            httpContext.HttpContext.Request.Headers[CorrelationConstants.CorrelationId].Should().HaveCountGreaterThan(0);
        }
    }
}
