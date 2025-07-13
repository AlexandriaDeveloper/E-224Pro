namespace Shared.Contracts.FormDetailsRequest;

public class GetFormDetailsRequest : Param
{
    public int? FormId { get; set; }
    public string? FormName { get; set; }
    public string? FormNum224 { get; set; }
    public string? FormNum55 { get; set; }
    public int? CollageId { get; set; }
    public int? FundId { get; set; }
    public int? DailyId { get; set; }
    public string? AuditorName { get; set; }
    public string? Details { get; set; }
    public string? CollageName { get; set; }
    public string? FundName { get; set; }
    public int? AccountId { get; set; }
    public string? AccountName { get; set; }
    //   public string? AccountNumber { get; set; }
    public decimal? Debit { get; set; }
    public decimal? Credit { get; set; }
    public string AccountType { get; set; } = string.Empty;


    public DateOnly? StartFrom { get; set; }
    public DateOnly? EndTo { get; set; }

}

public class GetAccountDownloadTemplateRequest
{
    public List<AccountsDebitCreditDto> Accounts { get; set; } = new List<AccountsDebitCreditDto>();



}
public class AccountsDebitCreditDto
{
    public string? CreditAccountName { get; set; }
    public int? CreditAccountNumber { get; set; }
    public string? DebitAccountName { get; set; }
    public int? DebitAccountNumber { get; set; }
}