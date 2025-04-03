using Core.Models;

public class SubsidiaryJournalDto
{


    public SubsidiaryJournalDto(SubsidiaryJournal existingSubsidiaryJournal)
    {
        Id = existingSubsidiaryJournal.Id;
        FormDetailsId = existingSubsidiaryJournal.FormDetailsId;
        SubAccountId = existingSubsidiaryJournal.SubAccountId;
        SubAccountName = existingSubsidiaryJournal.SubAccount != null ? existingSubsidiaryJournal.SubAccount.SubAccountName : null;
        SubAccountCode = existingSubsidiaryJournal.SubAccount != null ? existingSubsidiaryJournal.SubAccount.SubAccountNumber : null;
        Amount = existingSubsidiaryJournal.Amount;
        CollageId = existingSubsidiaryJournal.CollageId;
        CollageName = existingSubsidiaryJournal.Collage != null ? existingSubsidiaryJournal.Collage.CollageName : null;
        FundId = existingSubsidiaryJournal.FundId;
        FundName = existingSubsidiaryJournal.Fund != null ? existingSubsidiaryJournal.Fund.FundName : null;
        TransactionSide = existingSubsidiaryJournal.TransactionSide;
        AccountType = existingSubsidiaryJournal.AccountType;
        AccountItem = existingSubsidiaryJournal.AccountItem;
    }
    public SubsidiaryJournalDto()
    { }

    public int? Id { get; set; }
    public int? FormDetailsId { get; set; }
    public int? SubAccountId { get; set; }
    public string? SubAccountName { get; set; }
    public string? SubAccountCode { get; set; }
    public decimal? Amount { get; set; }

    public int? CollageId { get; set; }
    public string? CollageName { get; set; }
    public int? FundId { get; set; }
    public string? FundName { get; set; }
    // Account  Credit or debit 
    //جانب المعامله مدين او دائن

    public string TransactionSide { get; set; } = "Credit";

    // PayRoll or General
    public string? AccountType { get; set; }
    //State Expensess or Special Funds
    public string? AccountItem { get; set; }

    public SubsidiaryJournal ToEntity()
    {
        return new SubsidiaryJournal
        {
            Id = Id.HasValue ? Id.Value : 0,
            FormDetailsId = FormDetailsId.HasValue ? FormDetailsId.Value : 0,
            SubAccountId = SubAccountId.HasValue ? SubAccountId.Value : 0,
            Amount = Amount,
            CollageId = CollageId,
            FundId = FundId,
            TransactionSide = TransactionSide,
            AccountType = AccountType,
            AccountItem = AccountItem
        };
    }
}