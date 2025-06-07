using System;
using Core.Interfaces.Repository;
using Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Persistence.Specification;
using Shared.Contracts.FormDetailsRequest;
using Shared.Contracts.FormRequests;
using Shared.DTOs;
using Shared.DTOs.FormDetailsDtos;
using Shared.DTOs.FormDtos;
namespace Application.Services;

public class FormService
{
    private readonly IDailyRepository _dailyRepository;
    private readonly IFormRepository _formRepository;
    private readonly IAccountRepository _accountRepository;
    private readonly IFormDetailsRepository _formDetailsRepository;
    private readonly FormDetailsService _formDetailsService;
    private readonly IUow _uow;
    public FormService(IDailyRepository dailyRepository,
        IFormRepository formRepository, IAccountRepository accountRepository, IFormDetailsRepository formDetailsRepository, FormDetailsService formDetailsService, IUow uow)
    {
        _dailyRepository = dailyRepository;
        _formRepository = formRepository;
        _accountRepository = accountRepository;
        _formDetailsRepository = formDetailsRepository;
        _formDetailsService = formDetailsService;
        _uow = uow;
    }

    //Post Form
    public async Task<int> CreateFormAsync(PostFormRequest request, CancellationToken cancellationToken = default)
    {
        if (request == null)
        {
            throw new ArgumentNullException(nameof(request));
        }

        var form = new Core.Models.Form()
        {

            FormName = request.FormName,
            CollageId = request.CollageId,
            FundId = request.FundId,
            Num224 = request.Num224,
            Num55 = request.Num55,
            DailyId = request.DailyId,
            AuditorName = request.AuditorName,
            Details = request.Details,

        };
        await _formRepository.AddAsync(form);
        await _uow.CommitAsync(cancellationToken);

        List<FormDetails> FormDetails2 = new List<FormDetails>();
        FormDetails2 = request.FormDetails.Select(x => new FormDetails()
        {
            FormId = form.Id,
            Credit = x.Credit,
            Debit = x.Debit,
            AccountId = x.AccountId.HasValue ? x.AccountId.Value : 0,


        }).ToList();

        form.TotalDebit = FormDetails2.Sum(x => x.Debit);
        form.TotalCredit = FormDetails2.Sum(x => x.Credit);


        await _formDetailsRepository.AddRange2Async(FormDetails2);
        await _uow.CommitAsync(cancellationToken);


        return form.Id;
    }
    public async Task<int> TestCreate200FormAsync(CancellationToken cancellationToken = default)
    {

        List<Form> forms = new List<Form>();
        for (int x = 1; x < 1000; x++)
        {
            for (int i = 0; i < 200; i++)
            {
                // var Random = new Random().Next(1, 1000);
                var Random2 = new Random().Next(1, 6);

                var form = new Core.Models.Form()
                {
                    FormName = $"Form {i + 1}",
                    CollageId = 1,
                    FundId = Random2,
                    Num224 = (x * i).ToString(),
                    Num55 = (x * i).ToString(),
                    DailyId = x + 1,
                    AuditorName = "Mohamed Ali",
                    Details = "Hello world"
                };
                forms.Add(form);

            }

        }
        await _formRepository.AddRange2Async(forms);
        await _uow.CommitAsync(cancellationToken);
        return 1;
    }

    public async Task DeletFormAsync(int id, CancellationToken cancellationToken)
    {
        var form = await _formRepository.GetById(id);
        if (form == null)
        {
            throw new ArgumentNullException(nameof(form));
        }
        _formRepository.Delete(form);
        await _uow.CommitAsync(cancellationToken);
        return;
    }

    //Get Form by Id
    public async Task<FormDto> GetFormByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var form = await _formRepository.GetById(id);
        if (form == null)
        {
            throw new Exception("Could not find");
        }
        var formDto = new FormDto(form);
        return formDto;
    }
    public async Task<GetFormBySpecResponse> GetFormBySpecAsync(GetFormRequest request)
    {
        GetFormBySpecResponse response = new GetFormBySpecResponse();

        if (request.DailyId.HasValue)
        {
            var daily = await _dailyRepository.GetById(request.DailyId.Value);
            response.Daily = new DailyDto(daily);
        }
        var spec = new GetFormSpecification(request);
        var forms = await _formRepository.GetAll(spec);


        var countSpec = new GetFormCountAsyncSpecification(request);
        var count = await _formRepository.CountAsync(countSpec);

        var formsList = new List<FormDto>();
        if (forms.Any())
        {
            response.FormDtos = forms.Select(form => new FormDto(form!)).ToList();
        }
        response.TotalCount = count;



        return response;

    }

    public async Task<List<FormDto>> GetFormBySpecWithFormDetailsAsync(GetFormDetailsRequest request, CancellationToken cancellationToken)
    {
        var accounts = await _accountRepository.GetAll(null, cancellationToken);
        var spec = new GetFormWithDetailsSpecification(request);
        var forms = await _formRepository.GetAll(spec);
        if (!forms.Any())
        {
            return null!;
        }
        var formDtos = forms.Select(form => new FormDto()
        {
            Id = form!.Id,
            FormName = form.FormName,
            CollageId = form.CollageId,
            FundId = form.FundId,
            Num224 = form.Num224!,
            Num55 = form.Num55!,
            DailyId = form.DailyId,
            AuditorName = form.AuditorName,
            Details = form.Details,
            TotalCredit = form.TotalCredit.HasValue ? form.TotalCredit : null,
            TotalDebit = form.TotalDebit.HasValue ? form.TotalDebit : null,




            FormDetailsDtos = form.FormDetails.Select(x => new FormDetailDto()
            {
                Id = x.Id,
                Credit = x.Credit,
                Debit = x.Debit,
                AccountId = x.AccountId,
                AccountNumber = x.Account!.AccountNumber,
                AccountName = x.Account.AccountName

            }).ToList()

        });
        return formDtos.ToList();

    }

    public async Task<FormDto> UpdateFormAsync(int id, PutFormRequest form, CancellationToken cancellationToken)
    {
        var formFromDb = await _formRepository.GetById(id);
        if (formFromDb == null)
        {
            throw new ArgumentNullException(nameof(form));
        }

        if (!form.FormName.IsNullOrEmpty())
        {
            formFromDb.FormName = form.FormName!;
        }
        if (form.CollageId.HasValue)
        {
            formFromDb.CollageId = form.CollageId.Value;
        }
        if (form.FundId.HasValue)
        {
            formFromDb.FundId = form.FundId.Value;
        }
        if (!form.Num224.IsNullOrEmpty())
        {
            formFromDb.Num224 = form.Num224!;
        }
        if (!form.Num55.IsNullOrEmpty())
        {
            formFromDb.Num55 = form.Num55!;
        }
        if (form.DailyId.HasValue)
        {
            formFromDb.DailyId = form.DailyId.Value;
        }
        if (!form.AuditorName.IsNullOrEmpty())
        {
            formFromDb.AuditorName = form.AuditorName;
        }
        if (!form.Details.IsNullOrEmpty())
        {
            formFromDb.Details = form.Details;
        }
        await _formRepository.UpdateAsync(formFromDb);

        await _uow.CommitAsync(cancellationToken);
        await _formDetailsService.UpdateFormDetailsAsync(id, form.FormDetails, cancellationToken);


        return new FormDto(formFromDb);

    }
}
