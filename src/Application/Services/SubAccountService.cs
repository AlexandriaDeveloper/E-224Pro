using Core.Interfaces.Repository;
using Microsoft.EntityFrameworkCore;
using Persistence.Specification;
using Shared.Common;
using Shared.Contracts;
using Shared.DTOs;
using Shared.DTOs.AccountDtos;

public class SubAccountService
{
    private readonly IAccountRepository _accountRepository;
    private readonly ISubAccountRepository _subAccountRepository;
    private readonly IDailyRepository _dailyRepository;

    private readonly IUow _uow;
    public SubAccountService(IAccountRepository accountRepository, ISubAccountRepository subAccountRepository, IDailyRepository dailyRepository, IUow uow)
    {
        _accountRepository = accountRepository;
        _subAccountRepository = subAccountRepository;

        _uow = uow;
    }

    public async Task<PaginatedResult<SubAccountDto>> GetSubAccountsAccounts(int accountId, GetSubAccountRequest request, CancellationToken cancellationToken = default)
    {

        var spec = new GetSubAccountSpecification(request);
        var specCount = new GetSubAccountCountAsyncSpecification(request);
        var subAccounts = await _subAccountRepository.GetSubAccountsByAccountId(accountId, cancellationToken, spec);
        var subAccountsCount = await _subAccountRepository.CountAsync(specCount);

        var subAccountsResponse = subAccounts.Select(x => new SubAccountDto
        {
            Id = x.Id,
            SubAccountName = x.SubAccountName,
            SubAccountNumber = x.SubAccountNumber,
            AccountId = x.AccountId,
            ParentAccountName = x.Account!.AccountName,
            ParentAccountNumber = x.Account.AccountNumber


        }).ToList();
        return PaginatedResult<SubAccountDto>.Create(subAccountsResponse, request.PageIndex, request.PageSize, subAccountsCount);

    }
}