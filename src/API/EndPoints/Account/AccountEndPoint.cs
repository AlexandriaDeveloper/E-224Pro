
using Application.Services;
using Shared.DTOs.AccountDtos;

public static class Accounts
{
    public static WebApplication MapAccountsEndPoint(this WebApplication app)
    {
        var accountsGroup = app.MapGroup("accounts").RequireAuthorization();


        // dailiesGroup.MapPost("/Create", CreateAsync);
        // dailiesGroup.MapPost("/TestCreate", TestCreateAsync);

        // dailiesGroup.MapPut("/Update/{id}", UpdateAsync);
        accountsGroup.MapGet("/", GetBySpecAsync);
        accountsGroup.MapGet("/AccountsWithSubsidaries", GetAccountsWithSubsidaries);
        // dailiesGroup.MapGet("/{id}", GetByIdAsync);
        // dailiesGroup.MapDelete("delete/{id}", DeleteAsync);
        return app;
    }

    private static async Task<IResult> GetBySpecAsync(AccountService service, [AsParameters] GetAccountRequest request)
    {
        var accounts = await service.GetAccounts(request);
        return TypedResults.Ok(accounts);

    }


    private static async Task<IResult> GetAccountsWithSubsidaries(AccountService service)
    {
        var accounts = await service.GetAccountsWithSubsidariesOnly();
        return TypedResults.Ok(accounts);

    }


}