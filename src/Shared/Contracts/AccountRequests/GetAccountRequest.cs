using Shared.Contracts;

public class GetAccountRequest : Param
{
    public int? Id { get; set; }
    public string? AccountNumber { get; set; }
    public string? AccountName { get; set; }

}