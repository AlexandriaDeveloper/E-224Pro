
using Application.Services;
using FastEndpoints;
using Shared.Contracts;

namespace API.EndPoints.Daily;

public static class Dailies
{
    public static WebApplication MapDailiesEndPoint(this WebApplication app)
    {
        var dailiesGroup = app.MapGroup("dailies");


        dailiesGroup.MapPost("/Create", CreateAsync);
        dailiesGroup.MapPost("/TestCreate", TestCreateAsync);

        dailiesGroup.MapPut("/Update/{id}", UpdateAsync);
        dailiesGroup.MapGet("/", GetBySpecAsync);
        dailiesGroup.MapGet("/{id}", GetByIdAsync);
        dailiesGroup.MapDelete("delete/{id}", DeleteAsync);
        return app;
    }

    private static async Task<IResult> DeleteAsync(DailyService service, int id)
    {

        await service.DeleteDailyAsync(id);
        return TypedResults.NoContent();
    }

    private async static Task<IResult> CreateAsync(DailyService service, PostDailyRequest request)
    {
        await service.CreateDailyAsync(request);
        return TypedResults.Created();
    }
    private async static Task<IResult> TestCreateAsync(DailyService service)
    {
        await service.TestCreate30000DailyAsync();
        return TypedResults.Created();
    }
    private static async Task<IResult> GetByIdAsync(DailyService service, int id)
    {
        var daily = await service.GetDailyById(id);
        return daily == null ? TypedResults.NotFound() : TypedResults.Ok(daily);

    }

    private static async Task<IResult> GetBySpecAsync(DailyService service, [AsParameters] GetDailyRequest request)
    {
        var dailies = await service.GetDailiesBySpec(request);
        return TypedResults.Ok(dailies);
    }
    private static async Task<IResult> UpdateAsync(int id, DailyService service, PutDailyRequest request)
    {
        await service.UpdateDailyAsync(id, request);
        return TypedResults.NoContent();
    }

}