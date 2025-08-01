using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace API.EndPoints.Auth
{
    public static class RoleEndpoint
    {
        public static WebApplication MapRoleEndPoint(this WebApplication app)
        {
            var roleGroup = app.MapGroup("api/auth/roles").RequireAuthorization();

            // Add role management endpoints here
            // Example: roleGroup.MapPost("/create", CreateRoleAsync);
          roleGroup.MapGet("/", GetRolesAsync);

            return app;
        }

        private static async Task<IResult> GetRolesAsync(RoleManager<IdentityRole> roleManager)
        {
            var roles = await roleManager.Roles.ToListAsync();
            return TypedResults.Ok(roles.Select(role => new
            {
                role.Id,
                role.Name
            }));
        }
    

        // Example method for creating a role
        // private static async Task<IResult> CreateRoleAsync(RoleService service, [FromBody] CreateRoleRequest request)
        // {
        //     var result = await service.CreateRoleAsync(request);
        //     return result ? TypedResults.Ok() : TypedResults.BadRequest();
        // }

    }
}