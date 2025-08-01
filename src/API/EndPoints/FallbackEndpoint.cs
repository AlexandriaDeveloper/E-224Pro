using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.EndPoints
{
    public static class FallbackEndpoint
    {
        public static WebApplication MapFallbackEndPoint(this WebApplication app)
        {

            var subsidiaryJournalGroup = app.MapGroup("Fallback/").AllowAnonymous();


            subsidiaryJournalGroup.MapGet("/", Index).AllowAnonymous();
            return app;
        }

        private static IResult Index(CancellationToken cancellationToken = default)
        {

            return Results.File(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "index.html"), "text/html");
        }
    }
}