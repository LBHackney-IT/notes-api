using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Routing;
using NotesApi.V1;
using NotesApi.V1.Controllers;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NotesApi.Tests.V1.Controllers
{
    [TestFixture]
    public class BaseControllerTests
    {
        private BaseController _classUnderTest;
        private ControllerContext _controllerContext;
        private HttpContext _httpContext;

        [SetUp]
        public void Setup()
        {
            _httpContext = new DefaultHttpContext();
            _classUnderTest = new BaseController();
            _controllerContext = new ControllerContext(new ActionContext(_httpContext, new RouteData(), new ControllerActionDescriptor()));
            _classUnderTest.ControllerContext = _controllerContext;
        }

        [Test]
        public void ShouldThrowExceptionIfNoCorrelationHeader()
        {
            _classUnderTest.Invoking(x => x.GetCorrelationId()).Should().Throw<KeyNotFoundException>().WithMessage("Request is missing a correlationId");
        }

        [Test]
        public void GetCorrelationShouldReturnCorrelationIdWhenExists()
        {
            _httpContext.Request.Headers.Add(CorrelationConstants.CorrelationId, "123");
            var response = _classUnderTest.GetCorrelationId();
            response.Should().BeEquivalentTo("123");
        }


    }
}
