
using Microsoft.AspNetCore.Mvc;
using Persistence.Specification;

public static class FundsEndpoint
{
    public static WebApplication MapFundsEndpoint(this WebApplication app)
    {
        var fundGroup = app.MapGroup("Funds");


        // formGroup.MapPost("/Creat", PostForm);
        // formGroup.MapPost("/AddForm", AddForm);
        fundGroup.MapPost("/", PostFund);
        fundGroup.MapPut("/{id}", PutFund);
        fundGroup.MapDelete("/{id}", DeleteFund);
        fundGroup.MapGet("/", GetBySpecAsync);
        // formGroup.MapGet("/FormWithDetail", GetBySpecWithFormDetailAsync);
        // formGroup.MapGet("/{id}", GetByIdAsync);
        return app;
    }

    private static async Task DeleteFund(int id, FundService service, CancellationToken cancellationToken = default)
    {
        await service.DeleteFund(id, cancellationToken);

    }

    private static async Task<IResult> PutFund(int id, FundService service, [FromBody] PutFundRequest request, CancellationToken cancellationToken = default)
    {
        var updatedFund = await service.UpdateFund(id, request, cancellationToken);
        // Assuming the update operation does not return a value, we can just return a 204 No Content response.
        // If you want to return the updated fund, you can modify the method to return it.
        return TypedResults.Ok(updatedFund);

    }

    private static async Task<IResult> GetBySpecAsync(FundService service, [AsParameters] GetFundRequest request)
    {
        var funds = await service.GetFundsWithSpecs(request);
        return TypedResults.Ok(funds);
    }
    private static async Task<IResult> PostFund(FundService service, [FromBody] PostFundRequest request, CancellationToken cancellationToken = default)
    {
        var fund = await service.CreateFund(request, cancellationToken);
        if (fund == null)
        {
            return TypedResults.BadRequest("Failed to create fund.");
        }
        return TypedResults.Ok(fund);
    }

}
