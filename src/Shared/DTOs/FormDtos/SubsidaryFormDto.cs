namespace Shared.DTOs.FormDtos;

public class SubsidaryFormResponse
{
    public List<SubsidaryFormDto> SubsidaryFormDtos { get; set; }
    public int TotalCount { get; set; }
}

public class SubsidaryFormDto
{
    public int Id { get; set; }
    public string FormName { get; set; } = string.Empty;
    public string Num224 { get; set; } = string.Empty;
    public string Num55 { get; set; } = string.Empty;
    public string FundName { get; set; } = string.Empty;
    public int FundId { get; set; }
    public int CollageId { get; set; }
    public int DailyId { get; set; }
    public string? AuditorName { get; set; }
    public string? Details { get; set; }
    public decimal? TotalCredit { get; set; }
    public decimal? TotalDebit { get; set; }
    public decimal? SubsidaryTotalCredit { get; set; }
    public decimal? SubsidaryTotalDebit { get; set; }
    public int FormDetailsId { get; set; }







}