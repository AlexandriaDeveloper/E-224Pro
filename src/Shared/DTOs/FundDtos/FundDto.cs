using Core.Models;

public class FundDto
{


    public FundDto(Fund fund)
    {
        Id = fund.Id;
        FundName = fund.FundName;
        FundCode = fund.FundCode;
        CollageId = fund.CollageId;

    }

    public int? Id { get; set; }
    public string? FundName { get; set; }
    public string? FundCode { get; set; }
    public int? CollageId { get; set; }
}