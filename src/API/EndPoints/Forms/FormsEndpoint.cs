using System;
using Application.Services;
using FastEndpoints;
using Shared.Contracts.FormDetailsRequest;
using Shared.Contracts.FormRequests;
using Shared.DTOs.FormDtos;

namespace API.Forms;

public static class Forms
{
    public static WebApplication MapFormsEndPoint(this WebApplication app)
    {
        var formGroup = app.MapGroup("Forms");


        formGroup.MapPost("/Creat", PostForm);
        formGroup.MapPost("/AddForm", AddForm);
        formGroup.MapPost("/TestCreat", Post200Form);
        formGroup.MapPut("/Update/{id}", PutForm);
        formGroup.MapDelete("/Delete/{id}", DeleteForm);
        formGroup.MapGet("/", GetBySpecAsync);
        formGroup.MapGet("/FormWithDetail", GetBySpecWithFormDetailAsync);
        formGroup.MapGet("/{id}", GetByIdAsync);
        return app;
    }

    private static async Task<IResult> DeleteForm(FormService service, int id, CancellationToken cancellationToken)
    {
        await service.DeletFormAsync(id, cancellationToken);
        return TypedResults.Ok();
    }

    private static async Task<IResult> PostForm(FormService service, PostFormRequest form, CancellationToken cancellationToken)
    {
        var id = await service.CreateFormAsync(form, cancellationToken);
        return TypedResults.Created();
    }
    private static async Task<IResult> AddForm(FormService service, PostFormRequest form, CancellationToken cancellationToken)
    {
        var id = await service.CreateFormAsync(form, cancellationToken);
        return TypedResults.Created();
    }
    private static async Task<IResult> Post200Form(FormService service, CancellationToken cancellationToken)
    {
        var id = await service.TestCreate200FormAsync(cancellationToken);
        return TypedResults.Created();
    }
    private static async Task<IResult> PutForm(FormService service, int id, PutFormRequest form, CancellationToken cancellationToken)
    {
        await service.UpdateFormAsync(id, form, cancellationToken);
        return TypedResults.Created();
    }
    private static async Task<IResult> GetBySpecAsync(FormService service, [AsParameters] GetFormRequest request, CancellationToken cancellationToken)
    {
        var form = await service.GetFormBySpecAsync(request);
        return form == null ? TypedResults.NotFound() : TypedResults.Ok(form);
    }
    private static async Task<IResult> GetBySpecWithFormDetailAsync(FormService service, [AsParameters] GetFormDetailsRequest request, CancellationToken cancellationToken)
    {
        var form = await service.GetFormBySpecWithFormDetailsAsync(request, cancellationToken);
        return form == null ? TypedResults.NotFound() : TypedResults.Ok(form);
    }

    private static async Task<IResult> GetByIdAsync(FormService service, int id, CancellationToken cancellationToken)
    {
        var form = await service.GetFormByIdAsync(id);
        return form == null ? TypedResults.NotFound() : TypedResults.Ok(form);
    }
}
