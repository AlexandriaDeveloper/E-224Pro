
using Application.Services;
using FastEndpoints;
using Shared.Contracts;

public static class SubsidiaryJournal
{
    public static WebApplication MapSubsidiaryJournalEndPoint(this WebApplication app)
    {
        var subsidiaryJournalGroup = app.MapGroup("SubsidiaryJournal/");


        //  subsidiaryJournalGroup.MapGet("/", GetSubsidiaryJournal);

        subsidiaryJournalGroup.MapGet("SubId/{subAccountId}/dailyId/{dailyId}", GetSubsidiaryFormsByDailyId);
        subsidiaryJournalGroup.MapGet("SubId/{subAccountId}", GetDailiesBySpecAsync);

        subsidiaryJournalGroup.MapPost("/Creat", PostSubsidiaryJournal);
        // subsidiaryJournalGroup.MapPost("/TestCreat", PostTestSubsidiaryJournal);
        // subsidiaryJournalGroup.MapPut("/Update/{id}", UpdateSubsidiaryJournal);
        subsidiaryJournalGroup.MapDelete("/Delete/{id}", DeleteSubsidiaryJournal);
        return app;
    }

    private static async Task<IResult> GetDailiesBySpecAsync(int accountId, SubSidaryDailyService service, [AsParameters] GetDailyRequest request)
    {
        var dailies = await service.GetSubsidaryDailiesBySpec(accountId, request);
        return TypedResults.Ok(dailies);
    }

    private static async Task<IResult> GetSubsidiaryFormsByDailyId(int subAccountId, int dailyId, SubSidaryDailyService service, [AsParameters] GetSubsidiaryFormsByDailyIdRequest request, CancellationToken cancellationToken = default)
    {
        var subsidiaryForms = await service.GetSubsidaryDailyFormsByDailyIdAndSubsidaryId(subAccountId, dailyId, request, cancellationToken);
        return TypedResults.Ok(subsidiaryForms);
    }

    private static async Task<IResult> DeleteSubsidiaryJournal(SubsidiaryJournalService service, int id, CancellationToken cancellationToken = default)
    {
        await service.DeleteSubsidiaryJournal(id, cancellationToken);
        return TypedResults.Ok();
    }

    private static async Task<IResult> PostSubsidiaryJournal(SubsidiaryJournalService service, int formDerailsId, SubsidiaryJournalDto subsidiaryJournalDto, CancellationToken cancellationToken)
    {
        await service.CreateSubsidiaryJournal(formDerailsId, subsidiaryJournalDto, cancellationToken);
        return TypedResults.Created();
    }
    // private static async Task<IResult> PostTestSubsidiaryJournal(SubsidiaryJournalService service, CancellationToken cancellationToken)
    // {
    //     await service.TestCreateSubsidiaryJournals(cancellationToken);
    //     return TypedResults.Created();
    // }
    // private static async Task<IResult> UpdateSubsidiaryJournal(SubSidaryDailyService service, int id, SubsidiaryJournalDto subsidiaryJournalDto, CancellationToken cancellationToken)
    // {
    //     await service.UpdateSubsidiaryJournal(id, subsidiaryJournalDto, cancellationToken);
    //     return TypedResults.Ok();
    // }
    // private static async Task<IResult> GetSubsidiaryJournal(SubSidaryDailyService service, [AsParameters] GetSubsidiaryJournalsRequest request, CancellationToken cancellationToken = default)
    // {
    //     var subsidiaryJournal = await service.GetSubsidiaryJournals(request, cancellationToken);
    //     return TypedResults.Ok(subsidiaryJournal);
    // }
}