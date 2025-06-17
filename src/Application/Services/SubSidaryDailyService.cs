using Core.Interfaces.Repository;
using Core.Models;
using Microsoft.EntityFrameworkCore;
using Persistence.Specification;
using Shared.Common;
using Shared.Contracts;
using Shared.DTOs;
using Shared.DTOs.FormDetailsDtos;
using Shared.DTOs.FormDtos;

public class SubSidaryDailyService
{
    private readonly IDailyRepository _dailyRepository;
    private readonly IFormRepository _formRepository;
    private readonly IFormDetailsRepository _formDetailsRepository;
    private readonly ISubAccountRepository _subAccountRepository;

    private readonly ISubsidiaryJournalRepository _subsidiaryJournalRepository;
    private readonly IUow _uow;
    public SubSidaryDailyService(IDailyRepository dailyRepository, IFormRepository formRepository, IFormDetailsRepository formDetailsRepository, ISubAccountRepository subAccountRepository, ISubsidiaryJournalRepository subsidiaryJournalRepository, IUow uow)
    {
        _dailyRepository = dailyRepository;
        this._formRepository = formRepository;
        this._formDetailsRepository = formDetailsRepository;
        this._subAccountRepository = subAccountRepository;
        this._subsidiaryJournalRepository = subsidiaryJournalRepository;

        _uow = uow;
    }

    public async Task<PaginatedResult<DailyDto>> GetDailiesBySpec(GetDailyRequest request, CancellationToken cancellationToken = default)
    {
        var spec = new DailySpecification(request);

        var dailies = await _dailyRepository.GetAll(spec, cancellationToken);
        var dailyCountSpec = new DailyCountAsyncSpecification(request);
        var dailyCountResult = await _dailyRepository.CountAsync(dailyCountSpec);

        // Return the daily
        // DailiesResponse dailiesResponse = new DailiesResponse(); //
        var dailiesResponse = dailies.Select(x => new DailyDto(x!)).ToList();


        return PaginatedResult<DailyDto>.Create(dailiesResponse, request.PageIndex, request.PageSize, dailyCountResult);

    }
    public async Task<PaginatedResult<DailyDto>> GetSubsidaryDailiesBySpec(int accountId, GetDailyRequest request, CancellationToken cancellationToken = default)
    {
        var spec = new DailySpecification(request);

        var dailies = _dailyRepository.GetQueryable(spec).Include(x => x.Forms).ThenInclude(x => x.FormDetails.Where(x => x.AccountId == accountId));
        var dailyCountSpec = new DailyCountAsyncSpecification(request);
        var dailyCountResult = await _dailyRepository.CountAsync(dailyCountSpec);

        // Return the daily

        var dailiesResponse = dailies.Select(x => new DailyDto(x!)).ToList();


        return PaginatedResult<DailyDto>.Create(dailiesResponse, request.PageIndex, request.PageSize, dailyCountResult);

    }
    public async Task<PaginatedResult<SubsidaryFormDto>> GetSubsidaryDailyFormsByDailyIdAndSubsidaryId(int subaccountId, int dailyId, GetSubsidiaryFormsByDailyIdRequest request, CancellationToken cancellationToken = default)
    {

        var spec = new GetSubsidaryFormsSpecification(request);


        var subsidaryDailyForms = await _formRepository.GetQueryable(spec)
        .Include(x => x.Daily)
        .Include(x => x.FormDetails)
        .ThenInclude(x => x.SubsidiaryJournals)
         .Where(x => x.DailyId == dailyId && x.FormDetails.Any(x => x.AccountId == subaccountId))
        .ToListAsync(cancellationToken);


        var subsidiaryForms = subsidaryDailyForms.Select(x =>
           {

               return new SubsidaryFormDto()
               {
                   Id = x.Id,
                   FormName = x.FormName,
                   TotalCredit = x.FormDetails.Where(x => x.AccountId == subaccountId).Sum(x => x.Credit),
                   TotalDebit = x.FormDetails.Where(x => x.AccountId == subaccountId).Sum(x => x.Debit),
                   SubsidaryTotalCredit = x.FormDetails.Where(x => x.AccountId == subaccountId).Sum(x => x.SubsidiaryJournals.Sum(x => x.Credit)),
                   SubsidaryTotalDebit = x.FormDetails.Where(x => x.AccountId == subaccountId).Sum(x => x.SubsidiaryJournals.Sum(x => x.Debit)),
                   FormDetailsId = x.FormDetails.Where(x => x.AccountId == subaccountId).FirstOrDefault()!.Id,

                   CollageId = x.CollageId.Value,

                   FundId = x.FundId.Value,
                   Num224 = x.Num224,
                   Num55 = x.Num55,


                   DailyId = x.DailyId,
                   AuditorName = x.AuditorName,
                   Details = x.Details,
               };
           }).ToList();


        return PaginatedResult<SubsidaryFormDto>.Create(subsidiaryForms, request.PageIndex, request.PageSize, subsidiaryForms.Count);
    }
}