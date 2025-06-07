
using Shared.Contracts.FormDetailsRequest;

namespace Shared.Contracts.FormRequests;


public class PutFormRequest
{
    public int? Id { get; set; }
    public string? FormName { get; set; }
    public int? CollageId { get; set; }
    public int? FundId { get; set; }
    public string? Num224 { get; set; }

    public string? Num55 { get; set; }
    public int? DailyId { get; set; }
    public string? AuditorName { get; set; }
    public string? Details { get; set; }
    public List<PutFormDetail>? FormDetails { get; set; } = new List<PutFormDetail>();

}
