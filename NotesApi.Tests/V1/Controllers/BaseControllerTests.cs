using FluentAssertions;
using Hackney.Core.Middleware;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Routing;
using NotesApi.V1;
using NotesApi.V1.Controllers;
using System.Collections.Generic;
using Xunit;

namespace NotesApi.Tests.V1.Controllers
{
    public class BaseControllerFacts
    {
        private readonly BaseController _sut;
        private readonly ControllerContext _controllerContext;
        private readonly HttpContext _stubHttpContext;

        public BaseControllerFacts()
        {
            _stubHttpContext = new DefaultHttpContext();
            _controllerContext = new ControllerContext(new ActionContext(_stubHttpContext, new RouteData(), new ControllerActionDescriptor()));
            _sut = new BaseController();

            _sut.ControllerContext = _controllerContext;
        }

        [Fact]
        public void GetCorrelationShouldThrowExceptionIfCorrelationHeaderUnavailable()
        {
            // Arrange + Act + Assert
            _sut.Invoking(x => x.GetCorrelationId())
                .Should().Throw<KeyNotFoundException>()
                .WithMessage("Request is missing a correlationId");
        }

        [Fact]
        public void GetCorrelationShouldReturnCorrelationIdWhenExists()
        {
            // Arrange
            _stubHttpContext.Request.Headers.Add(HeaderConstants.CorrelationId, "123");

            // Act
            var result = _sut.GetCorrelationId();

            // Assert
            result.Should().BeEquivalentTo("123");
        }


    }
}
