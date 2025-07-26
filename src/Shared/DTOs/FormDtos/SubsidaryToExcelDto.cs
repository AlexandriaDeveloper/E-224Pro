namespace Shared.DTOs.FormDtos;

public class SubsidaryToExcelDto
{
    public int FormDetailsId { get; set; }
    public string FormName { get; set; } = string.Empty;
    public string CollageName { get; set; } = string.Empty;
    public string FundName { get; set; } = string.Empty;
    public string Num55 { get; set; } = string.Empty;
    public string Num224 { get; set; } = string.Empty;
    public string AuditorName { get; set; } = string.Empty;
    public string Details { get; set; } = string.Empty;
    public decimal? TotalCredit { get; set; }
    public decimal? TotalDebit { get; set; }
    public List<SubsidaryAccountDto> SubsidaryAccountDtos { get; set; } = new List<SubsidaryAccountDto>();

    public class SubsidaryAccountDto
    {
        public int Id { get; set; }
        public string AccountName { get; set; } = string.Empty;
        public string AccountNumber { get; set; } = string.Empty;
        public decimal? Credit { get; set; }
        public decimal? Debit { get; set; }
    }
}
public class SubsidaryToExcelRequest
{
    public int? Id { get; set; }
    public int? CollageId { get; set; }
    public int? FundId { get; set; }
    public int? DailyId { get; set; }
    public int? AccountId { get; set; }

}