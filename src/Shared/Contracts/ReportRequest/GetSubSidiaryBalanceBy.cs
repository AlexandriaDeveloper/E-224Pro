namespace Shared.Contracts.ReportRequest;

public class GetSubSidiaryBalanceBy
{
    public int? Id { get; set; }
    public int? DailyId { get; set; }
    public int? FundId { get; set; }
    public int? CollageId { get; set; }

    public int? EntryType { get; set; }
    public DateOnly? StartDate { get; set; }
    public DateOnly? EndDate { get; set; }
    public int? FormDetailsId { get; set; }
    public int? FormId { get; set; }
    public int? SubAccountId { get; set; }
    public int? AccountId { get; set; }
    public DateOnly? SpecificDate { get; set; }
    public int? ByMonth { get; set; }
    // public string? FundName { get; set; }
    public string? AccountType { get; set; }
    public string? AccountItem { get; set; }
}