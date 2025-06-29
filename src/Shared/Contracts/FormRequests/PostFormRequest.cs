using System;

namespace Shared.Contracts.FormRequests;

public class PostFormRequest
{


    public string FormName { get; set; } = string.Empty;
    public int CollageId { get; set; }
    public int FundId { get; set; }
    public string Num224 { get; set; } = string.Empty;

    public string Num55 { get; set; } = string.Empty;
    public string? AuditorName { get; set; } = string.Empty;
    public string? Details { get; set; } = string.Empty;

    public int DailyId { get; set; }
    public int EntryType { get; set; } = 0; // Default to NormalEntry

    public List<PostFormDetails>? FormDetails { get; set; }
}

public class PostFormDetails
{
    public int FormId { get; set; }
    public int? AccountId { get; set; }
    public decimal? Debit { get; set; }
    public decimal? Credit { get; set; }
    public string? AccountType { get; set; }
}

