
using Persistence.Specification;

public static class FundsEndpoint
{
    public static WebApplication MapFundsEndpoint(this WebApplication app)
    {
        var formGroup = app.MapGroup("Funds");


        // formGroup.MapPost("/Creat", PostForm);
        // formGroup.MapPost("/AddForm", AddForm);
        // formGroup.MapPost("/TestCreat", Post200Form);
        // formGroup.MapPut("/Update/{id}", PutForm);
        // formGroup.MapDelete("/Delete/{id}", DeleteForm);
        formGroup.MapGet("/", GetBySpecAsync);
        // formGroup.MapGet("/FormWithDetail", GetBySpecWithFormDetailAsync);
        // formGroup.MapGet("/{id}", GetByIdAsync);
        return app;
    }

    private static async Task<IResult> GetBySpecAsync(FundService service, [AsParameters] GetFundRequest request)
    {
        var funds = await service.GetFundsWithSpecs(request);
        return TypedResults.Ok(funds);
    }
}
