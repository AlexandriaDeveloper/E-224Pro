using Core.Interfaces.Repository;
using Microsoft.EntityFrameworkCore;
using Persistence.Specification;
using Shared.DTOs.AccountDtos;

public class AccountService
{
    private readonly IAccountRepository _accountRepository;
    private readonly ISubAccountRepository _subAccountRepository;

    private readonly IUow _uow;
    public AccountService(IAccountRepository accountRepository, ISubAccountRepository subAccountRepository, IUow uow)
    {
        _accountRepository = accountRepository;
        _subAccountRepository = subAccountRepository;
        _uow = uow;
    }
    public async Task<List<AccountDto>> GetAccounts(GetAccountRequest request)
    {
        var accountSpec = new GetAccountSpecification(request);
        var accounts = await _accountRepository.GetAll(accountSpec);
        if (!accounts.Any())
        {
            return null;
        }
        return accounts.Select(x => new AccountDto()
        {
            Id = x.Id,
            AccountName = x.AccountName,
            AccountNumber = x.AccountNumber,
            AccountStatus = x.AccountStatus
        }).ToList();

    }

    public async Task<List<AccountDto>> GetAccountsWithSubsidariesOnly()
    {

        //get accounts only has sub accounts
        var accountsIds = await _subAccountRepository.GetQueryable().GroupBy(x => x.AccountId).ToListAsync();

        //get account only in accountsIds
        var listOfAccounts = new List<AccountDto>();
        foreach (var id in accountsIds)
        {

            var accounts = await _accountRepository.GetQueryable(null).FirstOrDefaultAsync(x => x.Id == id.Key);
            if (accounts != null)
            {
                listOfAccounts.Add(new AccountDto()
                {
                    Id = accounts.Id,
                    AccountName = accounts.AccountName,
                    AccountNumber = accounts.AccountNumber,
                    AccountStatus = accounts.AccountStatus
                });
            }
        }

        return listOfAccounts;
    }
}