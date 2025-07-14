using System;

namespace Shared.DTOs.ReportDtos;

public class ReportDto
{
    public string FundCode { get; set; } = string.Empty;
    public string? FundName { get; set; } = "All";
    public int? CollageId { get; set; } = null;
    public string? CollageName { get; set; } = "All";
    public string? AccountType { get; set; } = "All";
    public string? AccountItem { get; set; } = "All";

    public List<ReportDetailsDto> ReportDetailsDtos { get; set; } = new List<ReportDetailsDto>();

    // public static implicit operator ReportDto(ReportDto v)
    // {
    //     throw new NotImplementedException();
    // }
}
public class ReportDetailsDto
{
    public int AccountId { get; set; }
    // public string? AccountNumber { get; set; }
    public string? AccountName { get; set; }
    public AccountBalance? OpeningBalance { get; set; }
    public AccountBalance? MonthlyTransAction { get; set; }
    // i need proprety auto calculate the closing balance
    public AccountBalance? ClosingBalance => new AccountBalance
    {
        Credit = MonthlyTransAction?.Credit + OpeningBalance?.Credit > MonthlyTransAction?.Debit + OpeningBalance?.Debit ? MonthlyTransAction?.Credit + OpeningBalance?.Credit - (MonthlyTransAction?.Debit + OpeningBalance?.Debit) : 0,
        Debit = MonthlyTransAction?.Debit + OpeningBalance?.Debit > MonthlyTransAction?.Credit + OpeningBalance?.Credit ? MonthlyTransAction?.Debit + OpeningBalance?.Debit - (MonthlyTransAction?.Credit + OpeningBalance?.Credit) : 0,

    };

}

// create value object for credit and debit
public class AccountBalance
{
    public decimal? Credit { get; set; } = 0;
    public decimal? Debit { get; set; } = 0;
    public decimal? Balance => Debit - Credit;
}
