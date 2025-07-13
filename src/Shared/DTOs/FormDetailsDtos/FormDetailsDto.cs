using System;

namespace Shared.DTOs.FormDetailsDtos;

// public class FormDetailsDto
// {
//     public int Id { get; set; }
//     public int FormId { get; set; }
//     public string? FormName { get; set; }
//     public String? Num55 { get; set; }
//     public string? Num224 { get; set; }
//     public decimal? TotalDebit => FormDetails.Sum(x => x.Debit) ?? 0;
//     public decimal? TotalCredit => FormDetails.Sum(x => x.Credit) ?? 0;
//     public bool IsBalanced => TotalDebit == TotalCredit ? true : false;
//     public decimal? IsNotBalanced => IsBalanced == false ? TotalDebit - TotalCredit : null;
//     public List<FormDetailDto> FormDetails { get; set; } = new List<FormDetailDto>();


// }

public class FormDetailDto
{
    public int Id { get; set; }
    public int AccountId { get; set; }
    //public int? AccountNumber { get; set; }
    public string? AccountName { get; set; }
    public decimal? Debit { get; set; }
    public decimal? Credit { get; set; }

    public string AccountType { get; set; } = string.Empty;


}
