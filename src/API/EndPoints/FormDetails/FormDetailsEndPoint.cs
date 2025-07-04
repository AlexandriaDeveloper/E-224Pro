using System;
using Application.Services;
using Shared.Contracts.FormDetailsRequest;
using Shared.DTOs.FormDetailsDtos;
using Shared.DTOs.FormDtos;

namespace API.EndPoints.FormDetails;

public static class FormDetailsEndPoint
{
    public static WebApplication MapFormsDetailsEndPoint(this WebApplication app)
    {
        var formGroup = app.MapGroup("FormDetails").RequireAuthorization();


        formGroup.MapPost("/Creat", PostFormDetails).RequireAuthorization();

        formGroup.MapPost("/TestCreat", TestPostFormDetails).RequireAuthorization();
        formGroup.MapGet("/{formId}", GetByFormIdAsync).RequireAuthorization();
        formGroup.MapPut("/Update/{id}", PutFormDetails).RequireAuthorization();
        formGroup.MapDelete("/Delete/{id}", DeleteFormDetails).RequireAuthorization();
        formGroup.MapGet("/Transfer/{dailyId}", TransferToSubsidaryJournalAsync).RequireAuthorization();
        formGroup.MapGet("/", GetBySpecAsync).RequireAuthorization();
        // formGroup.MapGet("/{id}", GetByIdAsync);
        return app;
    }

    private static async Task<List<FormDetailDto>> GetByFormIdAsync(FormDetailsService service, int formId, CancellationToken cancellationToken = default)
    {
        return await service.GetByFormIdAsync(formId, cancellationToken);

    }

    private static async Task<IResult> PostFormDetails(FormDetailsService service, PostFormDetails formDetails, CancellationToken cancellationToken)
    {
        await service.CreateFormDetailsAsync(formDetails, cancellationToken);
        return TypedResults.Created();
    }

    private static async Task<IResult> PutFormDetails(FormDetailsService service, int id, PutFormDetailsRequest formDetails, CancellationToken cancellationToken)
    {
        await service.PutFormDetailAsync(id, formDetails, cancellationToken);
        return TypedResults.Created();
    }
    private static async Task<IResult> TestPostFormDetails(FormDetailsService service, CancellationToken cancellationToken)
    {

        await service.Create20FormDetailsAsync(cancellationToken);
        return TypedResults.Created();
    }
    private static async Task<IResult> GetBySpecAsync(FormDetailsService service, [AsParameters] GetFormDetailsRequest request, CancellationToken cancellationToken)
    {
        var formDetails = await service.GetBySpecAsync(request, cancellationToken);
        return formDetails == null ? TypedResults.NotFound() : TypedResults.Ok(formDetails);
    }
    private static async Task<IResult> DeleteFormDetails(FormDetailsService service, int id, CancellationToken cancellationToken)
    {
        await service.Delete(id, cancellationToken);
        return TypedResults.Ok();
    }

    private static async Task<IResult> TransferToSubsidaryJournalAsync(FormDetailsService service, int dailyId, CancellationToken cancellationToken)
    {
        await service.TransferToSubsidaryJournalAsync(dailyId, cancellationToken);
        return TypedResults.Ok();
    }


}
