using Core.Interfaces.Repository;
using Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using NPOI.SS.Formula.Eval;
using NPOI.SS.Formula.Functions;
using Persistence.Specification;
using Shared.Common;
using Shared.Contracts;
using Shared.DTOs;
using Shared.DTOs.FormDetailsDtos;
using Shared.DTOs.FormDtos;
using Shared.DTOs.ReportDtos; // Add this if SubsidaryDailyFormDetailsDto is in ReportDtos namespace

public class SubSidaryDailyService
{
    private readonly IDailyRepository _dailyRepository;
    private readonly IFormRepository _formRepository;
    private readonly IFormDetailsRepository _formDetailsRepository;
    private readonly ISubAccountRepository _subAccountRepository;
    private readonly ICollageRepository _collageRepository;
    private readonly IFundRepository _fundRepository;
    private readonly IAccountRepository _accountRepository;
    private readonly ISubsidiaryJournalRepository _subsidiaryJournalRepository;
    private readonly IUow _uow;
    public SubSidaryDailyService(IDailyRepository dailyRepository, IAccountRepository accountRepository, IFormRepository formRepository, IFundRepository fundRepository, IFormDetailsRepository formDetailsRepository, ISubAccountRepository subAccountRepository, ISubsidiaryJournalRepository subsidiaryJournalRepository, IUow uow, ICollageRepository collageRepository)
    {
        _dailyRepository = dailyRepository;
        this._formRepository = formRepository;
        this._formDetailsRepository = formDetailsRepository;
        this._subAccountRepository = subAccountRepository;
        this._subsidiaryJournalRepository = subsidiaryJournalRepository;
        this._collageRepository = collageRepository;
        this._fundRepository = fundRepository;
        this._accountRepository = accountRepository;

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

        var dailies = _dailyRepository.GetQueryable(spec).Include(x => x.Forms!).ThenInclude(x => x.FormDetails.Where(x => x.AccountId == accountId)).ThenInclude(x => x.SubsidiaryJournals);
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
        .Include(x => x.Collage)
        .Include(x => x.Fund)
        .Include(x => x.Daily)

        .Include(x => x.FormDetails)
        .ThenInclude(x => x.SubsidiaryJournals)
         .Where(x => x.DailyId == dailyId && x.FormDetails.Any(x => x.AccountId == subaccountId))
        .ToListAsync(cancellationToken); // Use ToListAsync to execute the query and get the results
        var specCount = new GetSubsidaryFormsCountSpecification(request);
        var subsidaryDailyFormsCount = await _formRepository.CountAsync(specCount, cancellationToken);

        var subsidiaryForms = subsidaryDailyForms.Select(x =>
           {

               return new SubsidaryFormDto()
               {
                   Id = x.Id,
                   FormName = x.FormName,
                   TotalCredit = x.FormDetails.Where(x => x.AccountId == subaccountId).Sum(x => x.Credit),
                   TotalDebit = x.FormDetails.Where(x => x.AccountId == subaccountId).Sum(x => x.Debit),
                   SubsidaryTotalCredit = x.FormDetails.Where(x => x.AccountId == subaccountId).Sum(x => x.SubsidiaryJournals?.Sum(x => x.Credit) ?? 0),
                   SubsidaryTotalDebit = x.FormDetails.Where(x => x.AccountId == subaccountId).Sum(x => x.SubsidiaryJournals?.Sum(x => x.Debit) ?? 0),
                   FormDetailsId = x.FormDetails.Where(x => x.AccountId == subaccountId).FirstOrDefault()!.Id,
                   CollageId = x.CollageId ?? 0,
                   CollageName = x.Collage?.CollageName ?? string.Empty,
                   FundId = x.FundId ?? 0,
                   FundName = x.Fund?.FundName ?? string.Empty,
                   Num224 = x.Num224 ?? string.Empty,
                   Num55 = x.Num55 ?? string.Empty,

                   DailyId = x.DailyId,
                   AuditorName = x.AuditorName,
                   Details = x.Details,
               };
           }).ToList();


        return PaginatedResult<SubsidaryFormDto>.Create(subsidiaryForms, request.PageIndex, request.PageSize, subsidaryDailyFormsCount);
    }

    public async Task<List<SubsidaryFormDetailsDto>> GetSubsidaryFormDetailsByFormDetailsId(int formDetailsId, int subaccountId, CancellationToken cancellationToken = default)
    {
        List<SubAccount> subs = _subAccountRepository.GetQueryable(null)
                .Where(x => x.AccountId == subaccountId)
                .AsNoTracking().ToList();
        // create 
        List<SubsidaryFormDetailsDto> subSidaryFormDetails = new List<SubsidaryFormDetailsDto>();

        foreach (var sub in subs)
        {
            //if subsidary has record with account id add it to subSidaryFormDetails else  add new subsidary form details with zero credit and debit
            var subsidaryFormDetails = await _subsidiaryJournalRepository.GetQueryable(null)
            .Include(x => x.SubAccount)
                .Where(x => x.FormDetailsId == formDetailsId && x.SubAccountId == sub.Id)
                .FirstOrDefaultAsync(cancellationToken);
            if (subsidaryFormDetails != null)
            {
                subSidaryFormDetails.Add(new SubsidaryFormDetailsDto()
                {
                    Id = subsidaryFormDetails.Id,
                    SubAccountId = subsidaryFormDetails.SubAccountId,
                    SubAccountName = subsidaryFormDetails.SubAccount?.SubAccountName ?? string.Empty,
                    SubAccountNumber = subsidaryFormDetails.SubAccount?.SubAccountNumber ?? string.Empty,
                    Credit = subsidaryFormDetails.Credit,
                    Debit = subsidaryFormDetails.Debit
                });
            }
            else
            {
                subSidaryFormDetails.Add(new SubsidaryFormDetailsDto()
                {
                    SubAccountId = sub.Id,
                    SubAccountName = sub.SubAccountName ?? string.Empty,
                    SubAccountNumber = sub.SubAccountNumber ?? string.Empty,
                    Credit = 0,
                    Debit = 0

                });
            }




        }

        return subSidaryFormDetails;
    }

    public async Task<bool> AddOrUpdateSubsidaryFormDetail(AddOrUpdateSubsidaryFormDetailsRequest dto, CancellationToken cancellationToken = default)

    {
        if (dto == null) throw new ArgumentNullException(nameof(dto));

        // Get all existing entities for this FormDetailsId in one query
        var existingEntities = await _subsidiaryJournalRepository.GetQueryable(null)
            .Where(x => x.FormDetailsId == dto.FormDetailsId)
            .ToListAsync(cancellationToken);

        // get maxId 
        var maxId = await _subsidiaryJournalRepository.GetQueryable(null)
           .MaxAsync(x => (int?)x.Id) ?? 0;

        // Process each subsidiary form detail in the request
        foreach (var detailDto in dto.SubsidaryFormDetailsDtos)
        {
            // Find existing entity in the already-loaded list
            var entity = existingEntities.FirstOrDefault(x => x.SubAccountId == detailDto.SubAccountId);

            if (entity != null)
            {
                // Update existing entity
                entity.Credit = detailDto.Credit;
                entity.Debit = detailDto.Debit;
            }
            else
            {
                // Create new only if Credit or Debit has value > 0
                if ((detailDto.Credit.HasValue && detailDto.Credit > 0) ||
                    (detailDto.Debit.HasValue && detailDto.Debit > 0))
                {
                    var newEntity = new SubsidiaryJournal
                    {
                        Id = ++maxId, // Increment maxId for new entity
                        FormDetailsId = dto.FormDetailsId,
                        SubAccountId = detailDto.SubAccountId,
                        Credit = detailDto.Credit,
                        Debit = detailDto.Debit,
                    };
                    await _subsidiaryJournalRepository.AddAsync(newEntity, cancellationToken);
                }
                if (detailDto.Credit == 0 && detailDto.Debit == 0)
                {
                    // If both Credit and Debit are zero, remove the entity if it exists
                    var entityToRemove = existingEntities.FirstOrDefault(x => x.Id == detailDto.Id);
                    if (entityToRemove != null)
                    {
                        _subsidiaryJournalRepository.Delete(entityToRemove, cancellationToken);
                    }
                }
            }
        }
        await _uow.CommitAsync(cancellationToken);
        return true;
    }
    public async Task<SubsidaryDailyReportDto> GetSubsidaryDaily(GetSubsidartDailyRequest request)
    {
        var (collages, funds, accounts) = await LoadRequiredDataAsync();
        var spec = new GetSubsidaryDailyBySpecification(request);
        var subs = _formDetailsRepository.GetQueryable(spec).ToList();

        if (!subs.Any())
        {
            return BuildEmptyReportDto(request, collages, funds, accounts);
        }

        var subsResult = BuildSubsidiaryCollageReport(subs, collages, funds);
        var totalSubsidaries = CalculateTotalSubsidaries(subsResult);

        return BuildFinalReportDto(request, collages, funds, accounts, subs, subsResult, totalSubsidaries);
    }

    private async Task<(IEnumerable<Collage> collages, IEnumerable<Fund> funds, IEnumerable<Account> accounts)> LoadRequiredDataAsync()
    {
        var collages = (await _collageRepository.GetAll(null)).Where(c => c != null).Cast<Collage>().ToList();
        var funds = (await _fundRepository.GetAll(null)).Where(f => f != null).Cast<Fund>().ToList();
        var accounts = (await _accountRepository.GetAll(null)).Where(a => a != null).Cast<Account>().ToList();
        return (collages, funds, accounts);
    }

    private List<SubsidaryDailyCollageReportDto> BuildSubsidiaryCollageReport(List<FormDetails> subs, IEnumerable<Collage> collages, IEnumerable<Fund> funds)
    {
        return subs
            .GroupBy(x => x.Form?.CollageId)
            .Select(collageGroup =>
            {
                var collageId = collageGroup.Key;
                var collage = collageId.HasValue ? collages.SingleOrDefault(c => c.Id == collageId.Value) : null;
                return new SubsidaryDailyCollageReportDto
                {
                    CollageId = collageId ?? 0,
                    CollageName = collage?.CollageName ?? string.Empty,
                    Funds = collageGroup.GroupBy(y => y.Form?.FundId)
                        .Where(fundGroup => fundGroup.Key.HasValue)
                        .Select(fundGroup =>
                        {
                            var fundId = fundGroup.Key.Value;
                            var fund = funds.SingleOrDefault(f => f.Id == fundId);
                            return new SubsidaryDailyFundsReportDto()
                            {
                                AccountId = fundGroup.FirstOrDefault()?.AccountId ?? 0,
                                AccountName = fundGroup.FirstOrDefault()?.Account?.AccountName ?? string.Empty,
                                FundId = fundId,
                                FundName = fund?.FundName ?? string.Empty,
                                SubsidaryDetails = fundGroup
                                    .SelectMany(fd => fd.SubsidiaryJournals ?? new List<SubsidiaryJournal>())
                                    .GroupBy(j => j.SubAccountId)
                                    .Select(g =>
                                    {
                                        var subAccount = g.FirstOrDefault()?.SubAccount;
                                        return new SubsidaryDailyDetailsReportDto
                                        {
                                            SubsidaryId = g.Key,
                                            SubsidaryName = subAccount?.SubAccountName ?? string.Empty,
                                            SubsidaryNumber = subAccount?.SubAccountNumber ?? string.Empty,
                                            Credit = g.Sum(j => j.Credit ?? 0),
                                            Debit = g.Sum(j => j.Debit ?? 0)
                                        };
                                    }).OrderBy(x => x.SubsidaryNumber).ToList()
                            };
                        })
                        .Where(x => x != null)
                        .ToList()
                };
            }).ToList();
    }

    private List<SubsidaryDailyDetailsReportDto> CalculateTotalSubsidaries(List<SubsidaryDailyCollageReportDto> subsResult)
    {
        return subsResult
            .SelectMany(x => x.Funds)
            .SelectMany(x => x.SubsidaryDetails)
            .GroupBy(x => x.SubsidaryId)
            .Select(x => new SubsidaryDailyDetailsReportDto()
            {
                SubsidaryId = x.Key,
                SubsidaryName = x.FirstOrDefault()?.SubsidaryName ?? string.Empty,
                SubsidaryNumber = x.FirstOrDefault()?.SubsidaryNumber ?? string.Empty,
                Credit = x.Sum(j => j.Credit ?? 0),
                Debit = x.Sum(j => j.Debit ?? 0)
            }).OrderBy(x => x.SubsidaryNumber).ToList();
    }

    private SubsidaryDailyReportDto BuildEmptyReportDto(GetSubsidartDailyRequest request, IEnumerable<Collage> collages, IEnumerable<Fund> funds, IEnumerable<Account> accounts)
    {
        return new SubsidaryDailyReportDto()
        {
            CollageName = request.CollageId.HasValue ? collages.SingleOrDefault(x => x.Id == request.CollageId.Value)?.CollageName : "الكل",
            FundName = request.FundId.HasValue ? (funds.SingleOrDefault(x => x.Id == request.FundId.Value)?.FundName ?? string.Empty) : "الكل",
            Daily = string.Empty,
            AccountType = !string.IsNullOrEmpty(request.AccountType) ? request.AccountType : "الكل",
            AccountName = request.AccountId.HasValue ? (accounts.SingleOrDefault(x => x.Id == request.AccountId.Value)?.AccountName ?? "الكل") : "الكل",
            Collages = new List<SubsidaryDailyCollageReportDto>(),
            TotalSubsidaries = new List<SubsidaryDailyDetailsReportDto>()
        };
    }

    private SubsidaryDailyReportDto BuildFinalReportDto(GetSubsidartDailyRequest request, IEnumerable<Collage> collages, IEnumerable<Fund> funds, IEnumerable<Account> accounts, List<FormDetails> subs, List<SubsidaryDailyCollageReportDto> subsResult, List<SubsidaryDailyDetailsReportDto> totalSubsidaries)
    {
        return new SubsidaryDailyReportDto()
        {
            CollageName = request.CollageId.HasValue ? collages.Single(x => x.Id == request.CollageId.Value)?.CollageName : "الكل",
            FundName = request.FundId.HasValue ? (funds.SingleOrDefault(x => x.Id == request.FundId.Value)?.FundName ?? string.Empty) : "الكل",
            Daily = subs.FirstOrDefault()?.Form?.Daily?.Name,
            AccountType = !string.IsNullOrEmpty(request.AccountType) ? request.AccountType : "الكل",
            AccountName = request.AccountId.HasValue ? accounts.SingleOrDefault(x => x.Id == request.AccountId.Value)?.AccountName ?? "الكل" : "الكل",
            Collages = subsResult,
            TotalSubsidaries = totalSubsidaries
        };
    }
}