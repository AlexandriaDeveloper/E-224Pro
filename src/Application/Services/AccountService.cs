using Core.Interfaces.Repository;
using Persistence.Specification;
using Shared.DTOs.AccountDtos;

public class AccountService
{
    private readonly IAccountRepository _accountRepository;
    private readonly IUow _uow;
    public AccountService(IAccountRepository accountRepository, IUow uow)
    {
        _accountRepository = accountRepository;
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
}