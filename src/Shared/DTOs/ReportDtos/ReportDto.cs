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
}
public class ReportDetailsDto
{
    public int AccountId { get; set; }
    public string? AccountNumber { get; set; }
    public string? AccountName { get; set; }
    public decimal? Debit { get; set; }
    public decimal? Credit { get; set; }
    public decimal? Balance => Credit - Debit;

}
