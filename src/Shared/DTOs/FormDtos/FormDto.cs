using System;
using Core.Models;
using Shared.DTOs.FormDetailsDtos;


namespace Shared.DTOs.FormDtos;


public class GetFormBySpecResponse
{
    public DailyDto? Daily { get; set; }
    public List<FormDto>? FormDtos { get; set; } = new List<FormDto>();

    public int? TotalCount { get; set; } = 0;

}

public class FormDto
{
    public int Id { get; set; }
    public string FormName { get; set; } = string.Empty;
    public int? CollageId { get; set; }
    public string? CollageName { get; set; }
    public int? FundId { get; set; }
    public string? FundName { get; set; }
    public string Num224 { get; set; } = string.Empty;
    public string? AuditorName { get; set; }
    public string? Details { get; set; }
    public int EntryType { get; set; }

    public string Num55 { get; set; } = string.Empty;
    public int DailyId { get; set; }
    public decimal? TotalCredit { get; set; }
    public decimal? TotalDebit { get; set; }
    public decimal? TotalCredit2 => FormDetailsDtos != null ? FormDetailsDtos.Sum(x => x.Credit) : null;

    public decimal? TotalDebit2 => FormDetailsDtos != null ? FormDetailsDtos.Sum(x => x.Debit) : null;
    public bool IsBalanced => TotalCredit == TotalDebit;
    public List<FormDetailDto>? FormDetailsDtos { get; set; }


    public FormDto()
    {

    }
    public FormDto(Core.Models.Form form)
    {
        Id = form.Id;

        FormName = form.FormName;
        CollageId = form.CollageId.HasValue ? form.CollageId.Value : null;
        CollageName = form.Collage?.CollageName;
        FundId = form.FundId.HasValue ? form.FundId.Value : null;
        FundName = form.Fund?.FundName;
        Num224 = form.Num224;
        Num55 = form.Num55;
        DailyId = form.DailyId;
        AuditorName = form.AuditorName;
        Details = form.Details;
        TotalCredit = form.TotalCredit;
        TotalDebit = form.TotalDebit;
        // FormDetailsDtos = form.FormDetails.Select(x => new FormDetailDto()
        // {
        //     AccountId = x.AccountId,
        //     AccountName = x.Account.AccountName,
        //     AccountNumber = x.Account.AccountNumber,
        //     AccountType = x.AccountType,

        //     Credit = x.Credit,
        //     Debit = x.Debit
        // }).ToList();
    }


    // FormDetailsDtos = form.FormDetails.Select(fd =>
    // new FormDetailDto()
    // {
    //     Id = fd.Id,
    //     Credit = fd.Credit,
    //     Debit = fd.Debit,
    //     AccountName = fd.Account.AccountName,
    //     AccountNumber = fd.Account.AccountNumber,
    //     AccountId = fd.AccountId,
    //     AccountType = "test account",
    // }
    // ).ToList();



    public Core.Models.Form ToForm()
    {
        return new Core.Models.Form
        {
            Id = Id,

            FormName = FormName,
            CollageId = CollageId,
            FundId = FundId,
            Num224 = Num224,
            Num55 = Num55,
            DailyId = DailyId,
            AuditorName = AuditorName,
            Details = Details,
        };
    }
}
