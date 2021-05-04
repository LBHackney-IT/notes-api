using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Primitives;
using Moq;
using NotesApi.V1;
using NotesApi.V1.Controllers;
using NotesApi.V1.Logging;
using System;
using System.Threading.Tasks;
using Xunit;

namespace NotesApi.Tests.V1.Logging
{
    public class LoggingScopeMiddlewareTests
    {
        private readonly string _correlationId = Guid.NewGuid().ToString();
        private readonly string _userId = Guid.NewGuid().ToString();

        private readonly HttpContext _httpContext;
        private readonly Mock<ILogger<LoggingScopeMiddleware>> _mockLogger;

        public LoggingScopeMiddlewareTests()
        {
            _httpContext = new DefaultHttpContext();
            _httpContext.Request.Headers.Add(Constants.CorrelationId, new StringValues(_correlationId));
            _httpContext.Request.Headers.Add(Constants.UserId, new StringValues(_userId));

            _mockLogger = new Mock<ILogger<LoggingScopeMiddleware>>();
        }

        [Fact]
        public async Task InvokeAsyncTestBeginsLoggingScope()
        {
            var sut = new LoggingScopeMiddleware(null);
            await sut.InvokeAsync(_httpContext, _mockLogger.Object).ConfigureAwait(false);

            var expectedState = $"CorrelationId: {_correlationId}; UserId: {_userId}";
            _mockLogger.Verify(x => x.BeginScope(It.Is<object>(y => y.ToString() == expectedState)), Times.Once());
        }
    }
}
