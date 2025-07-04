
using Application.Services;
using Shared.DTOs.AccountDtos;

public static class Collages
{
    public static WebApplication MapCollgaesEndPoint(this WebApplication app)
    {
        var collagesGroup = app.MapGroup("collages").RequireAuthorization();


        // dailiesGroup.MapPost("/Create", CreateAsync);
        // dailiesGroup.MapPost("/TestCreate", TestCreateAsync);

        // dailiesGroup.MapPut("/Update/{id}", UpdateAsync);
        collagesGroup.MapGet("/", GetCollages);
        // dailiesGroup.MapGet("/{id}", GetByIdAsync);
        // dailiesGroup.MapDelete("delete/{id}", DeleteAsync);
        return app;
    }

    private static async Task<IResult> GetCollages(CollageService service)
    {
        var accounts = await service.GetCollages();
        return TypedResults.Ok(accounts);

    }
}