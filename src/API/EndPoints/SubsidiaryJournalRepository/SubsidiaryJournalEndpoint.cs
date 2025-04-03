
using Application.Services;
using FastEndpoints;

public static class SubsidiaryJournal
{
    public static WebApplication MapSubsidiaryJournalEndPoint(this WebApplication app)
    {
        var subsidiaryJournalGroup = app.MapGroup("SubsidiaryJournal/");


        subsidiaryJournalGroup.MapGet("/", GetSubsidiaryJournal);

        subsidiaryJournalGroup.MapPost("/Creat", PostSubsidiaryJournal);
        subsidiaryJournalGroup.MapPost("/TestCreat", PostTestSubsidiaryJournal);
        subsidiaryJournalGroup.MapPut("/Update/{id}", UpdateSubsidiaryJournal);
        subsidiaryJournalGroup.MapDelete("/Delete/{id}", DeleteSubsidiaryJournal);
        return app;
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
    private static async Task<IResult> PostTestSubsidiaryJournal(SubsidiaryJournalService service, CancellationToken cancellationToken)
    {
        await service.TestCreateSubsidiaryJournals(cancellationToken);
        return TypedResults.Created();
    }
    private static async Task<IResult> UpdateSubsidiaryJournal(SubsidiaryJournalService service, int id, SubsidiaryJournalDto subsidiaryJournalDto, CancellationToken cancellationToken)
    {
        await service.UpdateSubsidiaryJournal(id, subsidiaryJournalDto, cancellationToken);
        return TypedResults.Ok();
    }
    private static async Task<IResult> GetSubsidiaryJournal(SubsidiaryJournalService service, [AsParameters] GetSubsidiaryJournalsRequest request, CancellationToken cancellationToken = default)
    {
        var subsidiaryJournal = await service.GetSubsidiaryJournals(request, cancellationToken);
        return TypedResults.Ok(subsidiaryJournal);
    }
}