using System;
using Core.Interfaces.Repository;
using Core.Models;
using Microsoft.EntityFrameworkCore;
using Persistence.Specification;
using Shared.Constants.Enums;
using Shared.Contracts.FormDetailsRequest;
using Shared.DTOs.FormDetailsDtos;
using Shared.DTOs.FormDtos;

namespace Application.Services;

public class FormDetailsService
{

    private readonly IAccountRepository _accountRepository;
    private readonly IDailyRepository _dailyRepository;
    private readonly ISubsidiaryJournalRepository _subsidiaryJournalRepository;
    private readonly IFormRepository _formRepository;
    private readonly IFormDetailsRepository _formDetailsRepository;

    private readonly IUow _uow;
    public FormDetailsService(
        IDailyRepository dailyRepository,
        ISubsidiaryJournalRepository subsidiaryJournalRepository,
        IFormRepository formRepository,
        IFormDetailsRepository formDetailsRepository,
      IAccountRepository accountRepository, IUow uow)
    {
        this._dailyRepository = dailyRepository;
        this._subsidiaryJournalRepository = subsidiaryJournalRepository;
        _formRepository = formRepository;
        _formDetailsRepository = formDetailsRepository;
        _accountRepository = accountRepository;

        _uow = uow;
    }
    public async Task<bool> CreateFormDetailsAsync(PostFormDetails request, CancellationToken cancellationToken = default)
    {
        if (request == null)
        {
            throw new ArgumentNullException(nameof(request));
        }
        List<FormDetails> FormDetails2 = new List<FormDetails>();

        request.FormDetails.ForEach(x =>
        {
            var formDetail = new FormDetails()
            {

                AccountId = x.AccountId,
                FormId = request.FormId,
                Debit = x.Debit.HasValue ? x.Debit : 0,
                Credit = x.Credit.HasValue ? x.Credit : 0,
            };


            FormDetails2.Add(formDetail);


        });
        var form = await _formRepository.GetById(request.FormId, false, cancellationToken);
        if (form == null)
        {
            return false;
        }
        await _formDetailsRepository.AddRange2Async(FormDetails2);

        await _uow.CommitAsync(cancellationToken);

        await UpdateTotalCount(request.FormId, cancellationToken);
        await _uow.CommitAsync(cancellationToken);

        return true;

    }

    public async Task<bool> Create20FormDetailsAsync(CancellationToken cancellationToken = default)
    {
        int id = 1;
        for (int i = 0; i < 30000; i++)
        {
            var formId = i + 1;
            var form = await _formRepository.GetById(formId, false, cancellationToken);
            var formDetails = new List<FormDetails>();

            if (form == null)
            {
                return false;
            }

            for (int f = 0; f < 4; f++)
            {

                var Random2 = new Random().Next(1, 100000);

                var formDetail = new FormDetails()
                {
                    Id = id,
                    AccountId = 41,
                    FormId = formId,
                    Debit = Random2,
                    Credit = null,
                    //  AccountType = "AccountType" + i,
                };
                id++;
                formDetails.Add(formDetail);

                var formDetail2 = new FormDetails()
                {
                    Id = id,
                    AccountId = 42,
                    FormId = formId,
                    Debit = null,
                    Credit = Random2,
                    //   AccountType = "AccountType" + i,
                };
                id++;
                formDetails.Add(formDetail2);

            }
            await _formDetailsRepository.AddRangeAsync(formDetails);


            form.TotalCredit = formDetails.Sum(x => x.Credit);
            form.TotalDebit = formDetails.Sum(x => x.Debit);

            await _formRepository.UpdateAsync(form);


        }

        await _uow.CommitAsync(cancellationToken);


        return true;



    }

    public async Task<List<FormDetailDto>> GetByFormIdAsync(int formId, CancellationToken cancellationToken)
    {
        var formDetails = await _formDetailsRepository.GetByFormId(formId);
        var result = new List<FormDetailDto>();

        result = formDetails!.Select(x => new FormDetailDto()
        {
            Id = x.Id,
            Debit = x.Debit,
            Credit = x.Credit,
            AccountId = x.AccountId,
            AccountName = x.Account!.AccountName,
            // AccountNumber = x.Account.Id

        }).ToList();
        return await Task.FromResult(result);

    }


    public async Task<List<FormDto>> GetBySpecAsync(GetFormDetailsRequest request, CancellationToken cancellationToken)
    {
        var accounts = await _accountRepository.GetAll(null, cancellationToken);


        var spec = new GetFormWithDetailsSpecification(request);
        var formDetails = await _formRepository.GetAll(spec, cancellationToken);
        if (!formDetails.Any())
        {
            return null!;
        }
        var formDetailsDto = new List<FormDto>();
        foreach (var formDetail in formDetails)
        {
            var formDetailDto = new FormDto()
            {
                Id = formDetail!.Id,
                Num224 = formDetail.Num224!,
                FormName = formDetail.FormName,
                FundName = formDetail.Fund!.Name, //funds.FirstOrDefault(x => x.Id == formDetail.FundId)!.FundName,
                CollageId = formDetail.CollageId,
                CollageName = formDetail.Collage!.CollageName,//collages.FirstOrDefault(x => x.Id == formDetail.CollageId)!.CollageName,
                Num55 = formDetail.Num55!,
                AuditorName = formDetail.AuditorName,
                DailyId = formDetail.DailyId,
                TotalDebit = formDetail.TotalDebit,
                TotalCredit = formDetail.TotalCredit,
                FormDetailsDtos = formDetail.FormDetails.Select(x => new FormDetailDto()
                {

                    Id = x.Id,
                    Credit = x.Credit,
                    Debit = x.Debit,
                    AccountName = accounts.SingleOrDefault(acc => acc!.Id == x.AccountId)!.AccountName,
                    // AccountNumber = accounts.SingleOrDefault(acc => acc!.Id == x.AccountId)!.Id
                }).ToList()
            };
            formDetailsDto.Add(formDetailDto);
        }



        return formDetailsDto;

    }

    private async Task UpdateTotalCount(int formId, CancellationToken cancellationToken = default)
    {

        var spec = new GetFormSpecification(new Shared.Contracts.FormRequests.GetFormRequest()
        {
            Id = formId

        });
        spec.Includes.Add(x => x.FormDetails);
        var formFromDb = _formRepository.GetAll(spec, cancellationToken).Result.FirstOrDefault();
        if (formFromDb == null)
        {
            return;
        }
        formFromDb.TotalDebit = formFromDb!.FormDetails.Sum(x => x.Debit);
        formFromDb.TotalCredit = formFromDb.FormDetails.Sum(x => x.Credit);
        await _formRepository.UpdateAsync(formFromDb, cancellationToken);

        // await _uow.CommitAsync(cancellationToken);

    }

    private async Task UpdatePatchTotalCount(int[] formIds, CancellationToken cancellationToken = default)
    {

        var spec = new GetFormSpecification(new Shared.Contracts.FormRequests.GetFormRequest()
        {
            //Id = formId

        });
        spec.Includes.Add(x => x.FormDetails);
        var formsFromDb = await _formRepository.GetAll(spec, cancellationToken);
        if (formsFromDb == null)
        {
            return;
        }
        foreach (var form in formIds)
        {
            formsFromDb[form]!.TotalDebit = formsFromDb[form]!.FormDetails.Sum(x => x.Debit);
            formsFromDb[form]!.TotalCredit = formsFromDb[form]!.FormDetails.Sum(x => x.Credit);

        }

        // await _formRepository.UpdateAsync(formsFromDb, cancellationToken);

        // await _uow.CommitAsync(cancellationToken);

    }

    public async Task PutFormDetailAsync(int id, PutFormDetailsRequest formDetails, CancellationToken cancellationToken)
    {
        var formDetail = await _formRepository.GetQueryable().Where(x => x.Id == id).Include(x => x.FormDetails).FirstOrDefaultAsync();
        if (formDetail == null)
        {
            throw new ArgumentNullException(nameof(formDetail));
        }
        foreach (var item in formDetail.FormDetails)
        {
            var formDetailDto = formDetails.FormDetails.FirstOrDefault(x => x.Id == item.Id);
            if (formDetailDto != null)
            {
                item.Credit = formDetailDto.Credit;
                item.Debit = formDetailDto.Debit;
            }
        }
        await _formRepository.UpdateAsync(formDetail, cancellationToken);
        await _uow.CommitAsync(cancellationToken);
        await UpdateTotalCount(id, cancellationToken);
        await _uow.CommitAsync(cancellationToken);


    }
    //update list of  formdetailsrequest 
    //to compare between list and list from database if not exist delete it
    //if exist update it
    //if not exist add it
    //and updtae form total credit and debit 


    public async Task UpdateFormDetailsAsync(int formId, List<PutFormDetail> formDetails, CancellationToken cancellationToken)
    {
        var formDetailsFromDb = await _formDetailsRepository.GetByFormId(formId);
        var formDetailsToAdd = formDetails.Where(x => !formDetailsFromDb.Any(y => y.Id == x.Id)).ToList();
        var formDetailsToUpdate = formDetails.Where(x => formDetailsFromDb.Any(y => y.Id == x.Id)).ToList();
        var formDetailsToDelete = formDetailsFromDb.Where(x => !formDetails.Any(y => y.Id == x.Id)).ToList();
        List<FormDetails> FormDetailsList = new List<FormDetails>(); ;
        foreach (var item in formDetailsToAdd)
        {
            FormDetailsList.Add(new FormDetails()
            {

                AccountId = item.AccountId,
                FormId = formId,
                Debit = item.Debit,
                Credit = item.Credit
            });


        }
        if (FormDetailsList != null && FormDetailsList.Count > 0)
        {
            await _formDetailsRepository.AddRange2Async(FormDetailsList, cancellationToken);
        }
        foreach (var item in formDetailsToUpdate)
        {
            var formDetail = formDetailsFromDb.FirstOrDefault(x => x.Id == item.Id);
            if (formDetail != null)
            {
                formDetail.AccountId = item.AccountId;
                formDetail.Credit = item.Credit;
                formDetail.Debit = item.Debit;
            }
            await _formDetailsRepository.UpdateRangeAsync(formDetailsFromDb, cancellationToken);

        }
        foreach (var item in formDetailsToDelete)
        {
            var formDetail = formDetailsFromDb.FirstOrDefault(x => x.Id == item.Id);
            _formDetailsRepository.Delete(formDetail);

        }

        await _uow.CommitAsync(cancellationToken);
        await UpdateTotalCount(formId, cancellationToken);
        await _uow.CommitAsync(cancellationToken);

    }

    public async Task Delete(int id, CancellationToken cancellationToken)
    {
        var formDetail = await _formDetailsRepository.GetById(id);
        if (formDetail == null)
        {
            throw new ArgumentNullException(nameof(formDetail));
        }
        var form = await _formRepository.GetById(formDetail.FormId);
        if (form == null)
        {
            throw new ArgumentNullException(nameof(form));
        }
        form.FormDetails.Remove(formDetail);
        await UpdateTotalCount(form.Id);
        await _uow.CommitAsync(cancellationToken);

    }

    //Transfer To SubSidaryJournal
    public async Task TransferToSubsidaryJournalAsync(int dailyId, CancellationToken cancellationToken)
    {
        var rand = new Random();

        var spec = new TransfareDailyAsyncSpecification(dailyId);

        var daily = _dailyRepository.GetAll(spec).Result.FirstOrDefault();
        if (daily == null)
        {
            throw new ArgumentNullException(nameof(daily));
        }
        var subs = new List<SubsidiaryJournal>();
        foreach (var form in daily.Forms)
        {
            var formFromDb = await _formDetailsRepository.GetByFormId(form.Id);
            foreach (var FormDetail in formFromDb)
            {
                var sub = new SubsidiaryJournal()
                {
                    // SubAccountId = rand.Next(1, 2),
                    // TransactionSide = FormDetail.Credit.HasValue ? TransactionSideEnum.Credit.ToString() : TransactionSideEnum.Debit.ToString(),
                    // AccountItem = daily.AccountItem,
                    // AccountType = daily.DailyType,
                    FormDetailsId = FormDetail.Id,
                    // FundId = form.FundId,
                    // CollageId = form.CollageId,
                    Credit = null,
                    Debit = null





                };
                subs.Add(sub);

            }
        }
        await _subsidiaryJournalRepository.AddRange2Async(subs);
        await _uow.CommitAsync(cancellationToken);



    }



}
