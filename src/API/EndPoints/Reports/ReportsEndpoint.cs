using System;
using Application.Services;
using Shared.Contracts.ReportRequest;
using Shared.DTOs.FormDtos;
using Shared.DTOs.ReportDtos;

namespace API.EndPoints.Reports;

public static class ReportsEndpoint
{
    public static WebApplication MapReportEndPoint(this WebApplication app)
    {
        var formGroup = app.MapGroup("Reports").RequireAuthorization();


        // formGroup.MapPost("/Creat", PostForm);
        // formGroup.MapPost("/TestCreat", Post200Form);
        // formGroup.MapPut("/Update/{id}", PutForm);
        // formGroup.MapDelete("/Delete/{id}", DeleteForm);
        formGroup.MapGet("/ReportFormDetails", GetFormDetailsBySpecAsync).RequireAuthorization(); ;
        formGroup.MapGet("/ReportSubsidiaryJournalPdf", DownloadSubsidiaryReport).RequireAuthorization(); ;
        formGroup.MapGet("/ReportPdf", DownloadReports).RequireAuthorization(); ;

        // formGroup.MapGet("/FormWithDetail", GetBySpecWithFormDetailAsync);
        // formGroup.MapGet("/{id}", GetByIdAsync);
        return app;
    }

    private static async Task<IResult> GetFormDetailsBySpecAsync(ReportService service, [AsParameters] GetAccountsBalanceBy request, CancellationToken cancellationToken)
    {
        var result = await service.GetFormDetailsReportAsync(request, cancellationToken);
        return result == null ? TypedResults.NotFound() : TypedResults.Ok(result);
    }
    private static async Task<IResult> GetSubsidiaryJournalBySpecAsync(ReportService service, [AsParameters] GetSubSidiaryBalanceBy request, CancellationToken cancellationToken)
    {
        var result = await service.GetSubsidiaryReportAsync(request, cancellationToken);
        return result == null ? TypedResults.NotFound() : TypedResults.Ok(result);
    }

    private static async Task<IResult> DownloadReports(PDFReportService service, [AsParameters] GetAccountsBalanceBy request, CancellationToken cancellationToken)
    {
        var pdfBytes = await service.GenerateReport(request, cancellationToken);
        if (pdfBytes == null || pdfBytes.Length == 0)
        {
            return Results.NotFound("No data found for the specified report criteria.");
        }
        return Results.File(pdfBytes, "application/pdf", "report.pdf");
    }

    private static async Task<IResult> DownloadSubsidiaryReport(PDFReportService service, [AsParameters] GetSubsidartDailyRequest request, CancellationToken cancellationToken)
    {
        var pdfBytes = await service.GenerateSubsidaryReport(request, cancellationToken);
        if (pdfBytes == null || pdfBytes.Length == 0)
        {
            return Results.NotFound("No data found for the specified report criteria.");
        }
        return Results.File(pdfBytes, "application/pdf", "report.pdf");
    }

}
