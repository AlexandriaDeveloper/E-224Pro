using System.Threading.Tasks;
using Application.Services;
using Core.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Shared.DTOs.AuthDtos;
using Core.Interfaces.Repository;


public static class AuthEndpoints
{
    public static void MapAuthEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapPost("api/auth/login", async (UserManager<AppUser> userManager, TokenService tokenService, IUserAccountRepository userAccountRepository, [FromBody] LoginDto loginDto) =>
        {
            var user = await userManager.FindByNameAsync(loginDto.Username);
            if (user == null)
            {
                return Results.Unauthorized();
            }

            var result = await userManager.CheckPasswordAsync(user, loginDto.Password);
            if (!result)
            {
                return Results.Unauthorized();
            }

            var roles = await userManager.GetRolesAsync(user);
            IQueryable<UserAccount> userAccounts;
            if (roles.Contains("Admin"))
            {
                userAccounts = userAccountRepository.GetQueryable(null);
            }
            else
            {
                userAccounts = userAccountRepository.GetQueryable(null).Where(x => x.UserId == user.Id);
            }
            var token = tokenService.CreateToken(user, roles, userAccounts.ToList());

            return Results.Ok(new UserDto
            {
                DisplayName = user?.UserName ?? string.Empty,
                Token = token,
                Email = user?.Email ?? string.Empty
            });
        }).AllowAnonymous();

        app.MapPost("api/auth/register", async (UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager, TokenService tokenService, [FromBody] RegisterDto registerDto) =>
        {
            var user = new AppUser
            {
                UserName = registerDto.UserName,
                Email = registerDto.Email
            };

            var result = await userManager.CreateAsync(user, registerDto.Password);

            if (!result.Succeeded)
            {
                return Results.BadRequest(result.Errors);
            }

            if (!await roleManager.RoleExistsAsync(registerDto.Role))
            {
                await roleManager.CreateAsync(new IdentityRole(registerDto.Role));
            }
            await userManager.AddToRoleAsync(user, registerDto.Role);
            var roles = await userManager.GetRolesAsync(user);
            var token = tokenService.CreateToken(user, roles, new List<UserAccount>());

            return Results.Ok(new UserDto
            {
                DisplayName = user.UserName,
                Token = token,
                Email = user.Email
            });
        }).RequireAuthorization();
    }
}

