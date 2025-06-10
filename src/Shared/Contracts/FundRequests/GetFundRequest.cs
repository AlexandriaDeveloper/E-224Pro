public class GetFundRequest
{
    public int? Id { get; set; }
    public string? FundName { get; set; }
    public string? FundCode { get; set; }
    public int? CollageId { get; set; }
}

public class PostFundRequest
{
    public string FundName { get; set; } = string.Empty;
    public string FundCode { get; set; } = string.Empty;
    public int CollageId { get; set; }
}

public class PutFundRequest
{
    public string FundName { get; set; } = string.Empty;
    public string FundCode { get; set; } = string.Empty;
    public int CollageId { get; set; }
}