using System.Collections.Generic;
using Hackney.Core.Middleware;
using Microsoft.AspNetCore.Mvc;

namespace NotesApi.V2.Controllers
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
