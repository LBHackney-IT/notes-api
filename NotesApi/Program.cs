using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using System.Diagnostics.CodeAnalysis;

namespace NotesApi
{
    [ExcludeFromCodeCoverage]
    public static class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        // The generic host validates the container in Development. The previous web host did not.
        // Existing FluentValidation and test-fixture registrations fail that validation.
        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .UseDefaultServiceProvider((_, options) =>
                {
                    options.ValidateScopes = false;
                    options.ValidateOnBuild = false;
                })
                .ConfigureWebHostDefaults(webBuilder => webBuilder.UseStartup<Startup>());
    }
}
