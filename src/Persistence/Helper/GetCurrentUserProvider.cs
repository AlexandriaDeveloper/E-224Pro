using System;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;

namespace Persistence.Helper;

public class GetCurrentUserProvider
{
    private readonly IHttpContextAccessor _contexte;

    public GetCurrentUserProvider(IHttpContextAccessor contextAccessor)
    {
        _contexte = contextAccessor;
    }


    public string GetCurrentUserId()
    {
        if (_contexte.HttpContext?.User == null)
        {
            return "TestUser";
        }
        // TODO: Retrieve the current user's ID from the HttpContextAccessor
        // Example: return contexte.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        return _contexte.HttpContext!.User.FindFirst(ClaimTypes.NameIdentifier)?.Value; ; // Placeholder for a hardcoded user ID
    }
}
