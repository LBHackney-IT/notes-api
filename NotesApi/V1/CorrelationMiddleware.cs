using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NotesApi.V1
{
    public class CorrelationMiddleware
    {
        private readonly RequestDelegate _next;

        public CorrelationMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            if (context.Request.Headers["x-correlation-id"].Count == 0)
            {
                context.Request.Headers["x-correlation-id"] = Guid.NewGuid().ToString();
            }
            if (_next != null)
                await _next.Invoke(context).ConfigureAwait(false);
        }


    }

    public static class CorrelationMiddlewareExtension
    {
        public static IApplicationBuilder UseCorrelation(this IApplicationBuilder applicationBuilder)
        {
            return applicationBuilder.UseMiddleware<CorrelationMiddleware>();
        }
    }

}
