using System;
using Core.Interfaces.Repository;
using Core.Models;
using Microsoft.EntityFrameworkCore;
using NPOI.SS.Formula.Functions;
using Org.BouncyCastle.Math.EC.Rfc7748;
using Persistence.Specification;
using Shared.Contracts.ReportRequest;
using Shared.DTOs.FormDtos;
using Shared.DTOs.ReportDtos;

namespace Application.Services;

public class ReportService
{
    private readonly IFormRepository _formRepository;
    private readonly IDailyRepository _dailyRepository;
    private readonly ICollageRepository _collageRepository;
    private readonly IFundRepository _fundRepository;
    private readonly IFormDetailsRepository _formDetailsRepository;
    private readonly IAccountRepository _accountRepository;
    private readonly ISubsidiaryJournalRepository _subsidiaryJournalRepository;

    private readonly IUow _uow;
    public ReportService(IFormRepository formRepository,
    IAccountRepository accountRepository,
    IDailyRepository dailyRepository,
    ICollageRepository collageRepository,
    IFundRepository fundRepository,
    IFormDetailsRepository formDetailsRepository,
    ISubsidiaryJournalRepository subsidiaryJournalRepository,
    SubSidaryDailyService subsidaryDailyService,
    IUow uow)
    {
        _formRepository = formRepository;
        _accountRepository = accountRepository;
        _dailyRepository = dailyRepository;
        _collageRepository = collageRepository;
        _fundRepository = fundRepository;
        _formDetailsRepository = formDetailsRepository;
        _subsidiaryJournalRepository = subsidiaryJournalRepository;


        _uow = uow;
    }

    public async Task<ReportDto> GetFormDetailsReportAsync(GetAccountsBalanceBy getAccountsBalanceByAccount, CancellationToken cancellationToken = default)
    {
        if (getAccountsBalanceByAccount.DailyId.HasValue)
        {
            var daily = await _dailyRepository.GetById(getAccountsBalanceByAccount.DailyId.Value);
            getAccountsBalanceByAccount.DailyType = daily.DailyType;
            if (daily == null)
            {
                throw new ArgumentException("DailyId does not exist");
            }
            getAccountsBalanceByAccount.StartDate = daily.DailyDate;
            getAccountsBalanceByAccount.EndDate = daily.DailyDate;

        }
        if (getAccountsBalanceByAccount.StartDate == null)
        {
            throw new ArgumentException("StartDate is required");
        }

        if (getAccountsBalanceByAccount.EndDate == null)
        {
            getAccountsBalanceByAccount.EndDate = getAccountsBalanceByAccount.StartDate.Value;
        }

        var spec = new GetReportyBySpecification(getAccountsBalanceByAccount);
        var FullAccountsParam = new GetAccountsBalanceBy()
        {
            // StartDate = getAccountsBalanceByAccount.StartDate,
            EndDate = getAccountsBalanceByAccount.EndDate,
            CollageId = getAccountsBalanceByAccount.CollageId,
            FundId = getAccountsBalanceByAccount.FundId,
            DailyType = getAccountsBalanceByAccount.DailyType,
            AccountItem = getAccountsBalanceByAccount.AccountItem,
            EntryType = getAccountsBalanceByAccount.EntryType,
        };
        var spec2 = new GetReportyBySpecification(FullAccountsParam);
        var accountsBalance = await _formDetailsRepository.GetQueryable(spec2)
            .Include(x => x.Account)
            .Include(x => x.Form)
            .Include(x => x.Form!.Daily)

        .ToListAsync();
        var accountsResult = accountsBalance.GroupBy(x => x.AccountId);
        var reportDetailsList = new List<ReportDetailsDto>();

        foreach (var g in accountsResult)
        {
            var reportDetails = new ReportDetailsDto
            {
                AccountId = g.Key,
                AccountName = g.FirstOrDefault()!.Account!.AccountName,
                //       AccountNumber = g.FirstOrDefault()!.Account!.Id,
                OpeningBalance = await GetOpeningBalanceByAccount(g.Key, getAccountsBalanceByAccount, cancellationToken),
                MonthlyTransAction = await GetMonthlyAccountBalance(g.Key, getAccountsBalanceByAccount),
            };

            reportDetailsList.Add(reportDetails);
        }

        var accounType = "الكل";
        if (!string.IsNullOrEmpty(getAccountsBalanceByAccount.DailyType))
        {
            accounType = getAccountsBalanceByAccount.DailyType == "Payroll" ? "بيرول" : "عادية";
        }


        var result = new ReportDto
        {


            AccountType = accounType,
            AccountItem = getAccountsBalanceByAccount.AccountItem,
            FundName = getAccountsBalanceByAccount.FundId.HasValue ? (await _fundRepository.GetById(getAccountsBalanceByAccount.FundId.Value))?.FundName : "الكل",
            CollageName = getAccountsBalanceByAccount.CollageId.HasValue ? (await _collageRepository.GetById(getAccountsBalanceByAccount.CollageId.Value))?.CollageName : "الكل",

            ReportDetailsDtos = reportDetailsList.OrderBy(x => x.AccountNumber).ToList()
        };

        return result;
    }

    private async Task<AccountBalance?> GetMonthlyAccountBalance(int accountId, GetAccountsBalanceBy request)
    {
        var spec = new GetReportyBySpecification(request);
        var accountBalance = await _formDetailsRepository.GetQueryable(spec)
            .Include(x => x.Form)
            .Include(x => x.Form!.Daily)
            .Where(x => x.AccountId == accountId)
            .GroupBy(x => x.AccountId)
            .Select(g => new AccountBalance
            {
                Credit = g.Sum(x => x.Credit),
                Debit = g.Sum(x => x.Debit)
            }).FirstOrDefaultAsync();
        if (accountBalance == null)
        {
            return new AccountBalance
            {
                Credit = 0,
                Debit = 0
            };
        }
        return accountBalance;

    }

    private AccountBalance GetClosingBalanceByAccount(AccountBalance openingAccount, AccountBalance monthlyAccount)
    {
        var closingBalance = new AccountBalance
        {
            Credit = openingAccount.Credit + monthlyAccount.Credit,
            Debit = openingAccount.Debit + monthlyAccount.Debit
        };

        return closingBalance;
    }

    private async Task<AccountBalance?> GetOpeningBalanceByAccount(int AccountId, GetAccountsBalanceBy getAccountsBalanceByAccount, CancellationToken cancellationToken)
    {
        // Get the opening balance by account
        var GetOpeningBalanceByAccount = new GetAccountsBalanceBy()
        {
            EndDate = getAccountsBalanceByAccount.StartDate.Value.AddDays(-1), // Set the end date to one day before the start date
            CollageId = getAccountsBalanceByAccount.CollageId,
            FundId = getAccountsBalanceByAccount.FundId,
            DailyType = getAccountsBalanceByAccount.DailyType,
            AccountItem = getAccountsBalanceByAccount.AccountItem,

            //ByMonth = getAccountsBalanceByAccount.ByMonth,
            //ByYear = getAccountsBalanceByAccount.ByYear


        };


        var spec = new GetReportyBySpecification(GetOpeningBalanceByAccount);

        var reportSpec = await _formDetailsRepository.GetQueryable(spec)
            .Where(x => x.AccountId == AccountId)
            .Include(x => x.Form)
            .Include(x => x.Form!.Daily)
            .ToListAsync();

        if (!reportSpec.Any())
        {
            return new AccountBalance()
            {
                Credit = 0,
                Debit = 0
            };
        }
        var ByAccounts = reportSpec.GroupBy(x => x.AccountId);
        return ByAccounts.Select(g => new AccountBalance
        {
            Credit = g.Sum(x => x.Credit),
            Debit = g.Sum(x => x.Debit)
        }).FirstOrDefault();

    }

    public async Task<ReportDto> GetSubsidiaryReportAsync(GetSubSidiaryBalanceBy getSubSidiaryBalanceBy, CancellationToken cancellationToken = default)
    {
        var spec = new GetSubsidiaryReportyBySpecification(getSubSidiaryBalanceBy);
        var reportSpec = _subsidiaryJournalRepository.GetQueryable(spec, cancellationToken)
        .Include(x => x.SubAccount);
        if (!reportSpec.Any())
        {
            return new ReportDto()!;
        }

        // Calculate the total balance
        var ByAccounts = reportSpec.GroupBy(x => x.SubAccountId);
        Fund? fund = null;
        Collage? collage = null;

        //create 2 variables  in same line


        if (getSubSidiaryBalanceBy.FundId.HasValue)
        {
            fund = await _fundRepository.GetById(getSubSidiaryBalanceBy.FundId.Value);
        }
        if (getSubSidiaryBalanceBy.CollageId.HasValue)
        {
            collage = await _collageRepository.GetById(getSubSidiaryBalanceBy.CollageId.Value);
        }


        var result = new ReportDto()
        {


            AccountType = getSubSidiaryBalanceBy.AccountType,
            AccountItem = getSubSidiaryBalanceBy.AccountItem,
            ReportDetailsDtos = await ByAccounts.Select(g => new ReportDetailsDto()
            {
                AccountId = g.Key,
                AccountName = g.FirstOrDefault()!.SubAccount!.SubAccountName,
                AccountNumber = g.FirstOrDefault()!.SubAccount!.SubAccountNumber,
                // Credit = g.Where(x => x.TransactionSide == "Credit").Sum(x => x.Amount),
                // Debit = g.Where(x => x.TransactionSide == "Debit").Sum(x => x.Amount),

            }).ToListAsync()
        };
        if (fund != null)
        {
            result.FundName = fund.FundName;
            result.FundCode = fund.FundCode;
        }
        if (collage != null)
        {
            result.CollageName = collage.CollageName;
        }
        return result;
    }



}
