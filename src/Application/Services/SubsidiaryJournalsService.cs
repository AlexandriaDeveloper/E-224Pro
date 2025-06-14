
using System.Threading.Tasks;
using Core.Interfaces.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Shared.Contracts.ReportRequest;

public class SubsidiaryJournalService
{

    private readonly ISubsidiaryJournalRepository _subsidiaryJournalRepository;
    private readonly IFormDetailsRepository _formDetailsRepository;
    private readonly IUow _uow;
    public SubsidiaryJournalService(ISubsidiaryJournalRepository subsidiaryJournalRepository, IFormDetailsRepository formDetailsRepository, IUow uow)
    {
        _subsidiaryJournalRepository = subsidiaryJournalRepository;
        this._formDetailsRepository = formDetailsRepository;
        _uow = uow;
    }


    public async Task CreateSubsidiaryJournal(int formDetailsId, SubsidiaryJournalDto subsidiaryJournal, CancellationToken cancellationToken = default)
    {
        // Logic to create a new subsidiary journal record
        var formDetails = await _formDetailsRepository.GetQueryable().Where(x => x.Id == formDetailsId).Include(x => x.Form).ThenInclude(d => d.Daily)
        .FirstOrDefaultAsync();

        if (formDetails == null)
        {
            throw new ArgumentException("Invalid formDetailsId");
        }


        await _subsidiaryJournalRepository.AddAsync(
         new Core.Models.SubsidiaryJournal()
         {
             // Other mapping logic as needed
             // Example:
             SubAccountId = subsidiaryJournal.SubAccountId.Value,
             //...
             FormDetailsId = formDetailsId,
             Credit = subsidiaryJournal.Credit,
             Debit = subsidiaryJournal.Debit
             //  CollageId = formDetails.Form.CollageId,
             //  FundId = formDetails.Form.FundId,
             //  TransactionSide = formDetails.Credit.HasValue ? "Credit" : "Debit",
             //  AccountType = formDetails.Form.Daily.DailyType,
             //  AccountItem = formDetails.Form.Daily.AccountItem,
         }
        // Other mapping logic as needed

        );
        await _uow.CommitAsync(cancellationToken);
    }
    public async Task<GetSubsidiaryJournalResponse> GetSubsidiaryJournals(GetSubsidiaryJournalsRequest getSubsidiaryJournals, CancellationToken cancellationToken = default)
    {
        var spec = new GetSubsidiaryJournalsSpecification(getSubsidiaryJournals);
        var subsidiaryJournals = await _subsidiaryJournalRepository.GetAll(spec, cancellationToken);
        // Logic to retrieve the subsidiary journal records
        var subs = subsidiaryJournals.Select(sj => new SubsidiaryJournalDto(sj));
        return new GetSubsidiaryJournalResponse()
        {
            subsidiaryJournalDtos = subs.ToList()
        };

    }
    public async Task<SubsidiaryJournalDto> GetSubsidiaryJournalById(int id, CancellationToken cancellationToken = default)
    {
        var existingSubsidiaryJournal = await _subsidiaryJournalRepository.GetById(id);
        if (existingSubsidiaryJournal == null)
        {
            throw new ArgumentException("SubsidiaryJournal not found");
        }
        // Logic to retrieve the subsidiary journal record
        return new SubsidiaryJournalDto(existingSubsidiaryJournal);

    }

    public async Task DeleteSubsidiaryJournal(int id, CancellationToken cancellationToken)
    {
        var existingSubsidiaryJournal = await _subsidiaryJournalRepository.GetById(id);
        if (existingSubsidiaryJournal == null)
        {
            throw new ArgumentException("SubsidiaryJournal not found");
        }
        _subsidiaryJournalRepository.Delete(existingSubsidiaryJournal);
        await _uow.CommitAsync(cancellationToken);
    }

    public async Task UpdateSubsidiaryJournal(int id, SubsidiaryJournalDto subsidiaryJournal, CancellationToken cancellationToken = default)
    {
        var existingSubsidiaryJournal = await _subsidiaryJournalRepository.GetById(id);
        if (existingSubsidiaryJournal == null)
        {
            throw new ArgumentException("SubsidiaryJournal not found");
        }
        // Logic to update the existing subsidiary journal record
        if (subsidiaryJournal.Credit.HasValue)
            existingSubsidiaryJournal.Credit = subsidiaryJournal.Credit;
        if (subsidiaryJournal.Debit.HasValue)
            existingSubsidiaryJournal.Debit = subsidiaryJournal.Debit;
        // if (subsidiaryJournal.CollageId.HasValue)
        //     existingSubsidiaryJournal.CollageId = subsidiaryJournal.CollageId;
        // if (subsidiaryJournal.FundId.HasValue)
        //     existingSubsidiaryJournal.FundId = subsidiaryJournal.FundId;
        if (subsidiaryJournal.FormDetailsId.HasValue)
            existingSubsidiaryJournal.FormDetailsId = subsidiaryJournal.FormDetailsId.Value;
        if (subsidiaryJournal.SubAccountId.HasValue)
            existingSubsidiaryJournal.SubAccountId = subsidiaryJournal.SubAccountId.Value;
        // if (!subsidiaryJournal.AccountItem.IsNullOrEmpty())
        //     existingSubsidiaryJournal.AccountItem = subsidiaryJournal.AccountItem;
        // if (!subsidiaryJournal.AccountType.IsNullOrEmpty())
        //     existingSubsidiaryJournal.AccountType = subsidiaryJournal.AccountType;
        // if (!subsidiaryJournal.TransactionSide.IsNullOrEmpty())
        //     existingSubsidiaryJournal.TransactionSide = subsidiaryJournal.TransactionSide;
        // Logic to create a new subsidiary journal record

        await _subsidiaryJournalRepository.UpdateAsync(
         existingSubsidiaryJournal
        );
        await _uow.CommitAsync(cancellationToken);
    }

}