using System;

namespace Shared.Contracts.FormDetailsRequest;

public class PostFormDetails
{
    public int FormId { get; set; }
    public List<PostFormDetail> FormDetails { get; set; } = new List<PostFormDetail>();


}
public class PostFormDetail
{
    public int AccountId { get; set; }
    public decimal? Debit { get; set; }
    public decimal? Credit { get; set; }

}


public class PutFormDetailsRequest
{
    public int FormId { get; set; }
    public List<PutFormDetail> FormDetails { get; set; } = new List<PutFormDetail>();


}
public class PutFormDetail
{
    public int Id { get; set; }
    public int AccountId { get; set; }
    public decimal? Debit { get; set; }
    public decimal? Credit { get; set; }

}
