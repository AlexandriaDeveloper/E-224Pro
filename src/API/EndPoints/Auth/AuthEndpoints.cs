using System.Threading.Tasks;
using Application.Services;
using Core.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Shared.DTOs.AuthDtos;


public static class AuthEndpoints
{
    public static void MapAuthEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapPost("/auth/login", async (UserManager<AppUser> userManager, TokenService tokenService, [FromBody] LoginDto loginDto) =>
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
            var token = tokenService.CreateToken(user, roles);

            return Results.Ok(new UserDto
            {
                DisplayName = user.UserName,
                Token = token,
                Email = user.Email
            });
        });

        app.MapPost("auth/register", async (UserManager<AppUser> userManager, TokenService tokenService, [FromBody] RegisterDto registerDto) =>
        {
            var user = new AppUser
            {
                UserName = registerDto.DisplayName,
                Email = registerDto.Email
            };

            var result = await userManager.CreateAsync(user, registerDto.Password);

            if (!result.Succeeded)
            {
                return Results.BadRequest(result.Errors);
            }

            var roles = await userManager.GetRolesAsync(user);
            var token = tokenService.CreateToken(user, roles);

            return Results.Ok(new UserDto
            {
                DisplayName = user.UserName,
                Token = token,
                Email = user.Email
            });
        });
    }
}

