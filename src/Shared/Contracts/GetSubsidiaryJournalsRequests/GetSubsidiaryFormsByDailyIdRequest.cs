using Shared.Contracts;

public class GetSubsidiaryFormsByDailyIdRequest : Param
{
    public int? Id { get; set; }
    public int? AccountId { get; set; }
    public int? SubAccountId { get; set; }
    public int? DailyId { get; set; }
    public int? FormDetailsId { get; set; }
    public int? CollageId { get; set; }
    public int? FundId { get; set; }
}