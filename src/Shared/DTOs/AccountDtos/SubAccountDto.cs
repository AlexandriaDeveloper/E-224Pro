namespace Shared.DTOs.AccountDtos;

public class SubAccountDto
{
    public int? Id { get; set; }
    public string? SubAccountNumber { get; set; }
    public string? SubAccountName { get; set; }
    public string? ParentAccountName { get; set; }
    public string? ParentAccountNumber { get; set; }

    public int AccountId { get; set; }

}
