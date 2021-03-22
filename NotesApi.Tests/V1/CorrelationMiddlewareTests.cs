using FluentAssertions;
using Microsoft.AspNetCore.Http;
using NotesApi.V1;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NotesApi.Tests.V1
{
    [TestFixture]
    public class CorrelationMiddlewareTests
    {
        private CorrelationMiddleware _classUnderTest;

        [SetUp]
        public void Setup()
        {
            _classUnderTest = new CorrelationMiddleware(null);
        }

        [Test]
        public async Task AddCorrelationIdIfItDoesNotExist()
        {
            var httpContext = new DefaultHttpContext();
            await _classUnderTest.InvokeAsync(httpContext).ConfigureAwait(false);
            httpContext.HttpContext.Request.Headers[CorrelationConstants.CorrelationId].Should().HaveCountGreaterThan(0);
        }

        [Test]
        public async Task DoesNotAddNewCorrelationIdIfAlreadyExist()
        {
            var httpContext = new DefaultHttpContext();
            var correlationIdValue = "11234";
            httpContext.HttpContext.Request.Headers.Add(CorrelationConstants.CorrelationId, correlationIdValue);
            await _classUnderTest.InvokeAsync(httpContext).ConfigureAwait(false);
            httpContext.HttpContext.Request.Headers[CorrelationConstants.CorrelationId].Should().BeEquivalentTo(correlationIdValue);


        }
    }
}
