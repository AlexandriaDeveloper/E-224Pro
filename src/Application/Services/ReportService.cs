using System;
using Core.Interfaces.Repository;
using Core.Models;
using Microsoft.EntityFrameworkCore;
using Persistence.Specification;
using Shared.Contracts.ReportRequest;
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
        var spec = new GetReportyBySpecification(getAccountsBalanceByAccount);
        var reportSpec = _formDetailsRepository.GetQueryable(spec);
        if (!reportSpec.Any())
        {
            return new ReportDto()!;
        }

        // Calculate the total balance
        var ByAccounts = reportSpec.GroupBy(x => x.AccountId);
        Fund? fund = null;
        Collage? collage = null;

        //create 2 variables  in same line


        if (getAccountsBalanceByAccount.FunId.HasValue)
        {
            fund = await _fundRepository.GetById(getAccountsBalanceByAccount.FunId.Value);
        }
        if (getAccountsBalanceByAccount.CollageId.HasValue)
        {
            collage = await _collageRepository.GetById(getAccountsBalanceByAccount.CollageId.Value);
        }


        var result = new ReportDto()
        {


            AccountType = getAccountsBalanceByAccount.AccountType,
            AccountItem = getAccountsBalanceByAccount.AccountItem,
            ReportDetailsDtos = await ByAccounts.Select(g => new ReportDetailsDto()
            {
                AccountId = g.Key,
                AccountName = g.FirstOrDefault()!.Account!.AccountName,
                AccountNumber = g.FirstOrDefault()!.Account!.AccountNumber,
                Credit = g.Sum(x => x.Credit),
                Debit = g.Sum(x => x.Debit)
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
                Credit = g.Where(x => x.TransactionSide == "Credit").Sum(x => x.Amount),
                Debit = g.Where(x => x.TransactionSide == "Debit").Sum(x => x.Amount),

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
