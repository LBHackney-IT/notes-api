using Hackney.Core.Middleware;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace NotesApi.V1.Controllers
{
    public class BaseController : Controller
    {
        public string GetCorrelationId()
        {
            if (HttpContext.Request.Headers[HeaderConstants.CorrelationId].Count == 0)
                throw new KeyNotFoundException("Request is missing a correlationId");

            return HttpContext.Request.Headers[HeaderConstants.CorrelationId];
        }
    }
}
