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
        Debit = existingSubsidiaryJournal.Debit;
        Credit = existingSubsidiaryJournal.Credit;
        // CollageId = existingSubsidiaryJournal.CollageId;
        // CollageName = existingSubsidiaryJournal.Collage != null ? existingSubsidiaryJournal.Collage.CollageName : null;
        // FundId = existingSubsidiaryJournal.FundId;
        // FundName = existingSubsidiaryJournal.Fund != null ? existingSubsidiaryJournal.Fund.FundName : null;
        // TransactionSide = existingSubsidiaryJournal.TransactionSide;
        // AccountType = existingSubsidiaryJournal.AccountType;
        // AccountItem = existingSubsidiaryJournal.AccountItem;
    }
    public SubsidiaryJournalDto()
    { }

    public int? Id { get; set; }
    public int? FormDetailsId { get; set; }
    public int? SubAccountId { get; set; }
    public string? SubAccountName { get; set; }
    public string? SubAccountCode { get; set; }
    public decimal? Debit { get; set; }
    public decimal? Credit { get; set; }

    public int? CollageId { get; set; }
    public string? CollageName { get; set; }
    public int? FundId { get; set; }
    public string? FundName { get; set; }


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
            Debit = Debit,
            Credit = Credit

        };
    }
}

public class SubsidiaryFormDto
{
    public int Id { get; set; }
    public int DailyId { get; set; }
    public decimal? TotalCredit { get; set; }
    public decimal? TotalDebit { get; set; }
    public string FormName { get; set; }
    public string Num55 { get; set; }
    public string Num224 { get; set; }
    public int? CollageId { get; set; }
    public string? CollageName { get; set; }
    public int? FundId { get; set; }
    public string? FundName { get; set; }

    public string? AuditorName { get; set; }


}