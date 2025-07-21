namespace Shared.DTOs.FormDtos;

public class SearchFormDto
{
    public int Id { get; set; }
    public string FormName { get; set; } = string.Empty;
    public string CollageName { get; set; } = string.Empty;
    public string FundName { get; set; } = string.Empty;
    public string Num224 { get; set; } = string.Empty;
    public string Num55 { get; set; } = string.Empty;
    public int DailyId { get; set; }
    public string DailyName { get; set; } = string.Empty;
    public DateOnly DailyDate { get; set; }
    public string DailyType { get; set; } = string.Empty;
    public string AuditorName { get; set; } = string.Empty;
    public string Details { get; set; } = string.Empty;
    public string EntryType { get; set; }
}
