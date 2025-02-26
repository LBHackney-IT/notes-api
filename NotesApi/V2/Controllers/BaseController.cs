using System.Collections.Generic;
using Hackney.Core.Middleware;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace NotesApi.V2.Controllers
{
    public class BaseController : Controller
    {
        public BaseController()
        {
            ConfigureJsonSerializer();
        }
        public string GetCorrelationId()
        {
            if (HttpContext.Request.Headers[HeaderConstants.CorrelationId].Count == 0)
                throw new KeyNotFoundException("Request is missing a correlationId");

            return HttpContext.Request.Headers[HeaderConstants.CorrelationId];
        }
        public static void ConfigureJsonSerializer()
        {
            JsonConvert.DefaultSettings = () =>
            {
                var settings = new JsonSerializerSettings();
                settings.Formatting = Formatting.Indented;
                settings.ContractResolver = new CamelCasePropertyNamesContractResolver();

                settings.DateTimeZoneHandling = DateTimeZoneHandling.Utc;
                settings.DateFormatHandling = DateFormatHandling.IsoDateFormat;

                return settings;
            };
        }
    }
}
