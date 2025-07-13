using System.ComponentModel;
using Shared.Contracts;

public class GetSubAccountRequest : Param
{
    public int? Id { get; set; }
    public int? AccountId { get; set; }
    public string? ParentAccountName { get; set; }
    public int? ParentAccountId { get; set; }
    public string? SubAccountNumber { get; set; }
    public string? SubAccountName { get; set; }

}