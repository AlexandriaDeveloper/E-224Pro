using System;

namespace Shared.Contracts.FormRequests;

public class GetFormRequest : Param
{
    public int? Id { get; set; }
    public string? FormName { get; set; }
    public int? CollageId { get; set; }
    public int? FundId { get; set; }
    public string? Num224 { get; set; }
    public string? Num55 { get; set; }
    public int? DailyId { get; set; }
    public string? AuditorName { get; set; }
    public int? EntryType { get; set; } // 0 for normal entry, 1 for reversed entry
    public string? Details { get; set; }


}
