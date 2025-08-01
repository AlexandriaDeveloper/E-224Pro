public static class SubAccount
{

    public static WebApplication MapSubAccountsEndPoint(this WebApplication app)
    {
        var subAccountsGroup = app.MapGroup("api/SubAccount/{accountId}").RequireAuthorization(
            x => x.RequireRole("Admin", "SubsidaryAccountant")); ;


        // dailiesGroup.MapPost("/Create", CreateAsync);
        // dailiesGroup.MapPost("/TestCreate", TestCreateAsync);

        // dailiesGroup.MapPut("/Update/{id}", UpdateAsync);
        subAccountsGroup.MapGet("/", GetBySpecAsync).RequireAuthorization();
        //accountsGroup.MapGet("/AccountsWithSubsidaries", GetAccountsWithSubsidaries);
        // dailiesGroup.MapGet("/{id}", GetByIdAsync);
        // dailiesGroup.MapDelete("delete/{id}", DeleteAsync);
        return app;
    }
    private static async Task<IResult> GetBySpecAsync(int accountId, SubAccountService service, [AsParameters] GetSubAccountRequest request)
    {
        var accounts = await service.GetSubAccountsAccounts(accountId, request);
        return TypedResults.Ok(accounts);

    }



}