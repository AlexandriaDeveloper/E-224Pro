using System;

namespace Shared.Contracts.ReportRequest;

public class GetAccountsBalanceBy
{

    public int? DailyId { get; set; }
    public int? FunId { get; set; }
    public int? CollageId { get; set; }

    public DateOnly? StartDate { get; set; }
    public DateOnly? EndDate { get; set; }
    public int? FormId { get; set; }
    public DateOnly? SpecificDate { get; set; }
    public int? ByMonth { get; set; }
    // public string? FundName { get; set; }
    public string? AccountType { get; set; }
    public string? AccountItem { get; set; }
}
