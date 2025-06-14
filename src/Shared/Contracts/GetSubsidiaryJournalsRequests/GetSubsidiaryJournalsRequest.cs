using Shared.Contracts;

public class GetSubsidiaryJournalsRequest : Param
{

    public int? Id { get; set; }
    public int? FormDetailsId { get; set; }
    public int? CollageId { get; set; }
    public int? FundId { get; set; }

    public string? TransactionSide { get; set; }

    public int? SubAccountId { get; set; }
    public string? AccountType { get; set; }
    public string? AccountItem { get; set; }
    public DateOnly? DateFrom { get; set; }
    public DateOnly? DateTo { get; set; }
}
