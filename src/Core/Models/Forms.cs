using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Core.Constants;

namespace Core.Models;

public class Form : BaseEntity
{
    [NotMapped]
    public override string Name { get; set; } = string.Empty;
    [MaxLength(150)]
    public string FormName { get; set; } = string.Empty;
    public int? CollageId { get; set; }
    public int? FundId { get; set; }
    [MaxLength(150)]
    public string? Num224 { get; set; } = string.Empty;
    [MaxLength(150)]
    public string? Num55 { get; set; } = string.Empty;
    public decimal? TotalDebit { get; set; }
    public decimal? TotalCredit { get; set; }

    public int DailyId { get; set; }
    [MaxLength(150)]
    public string? AuditorName { get; set; }
    [MaxLength(500)]
    public string? Details { get; set; }
    [MaxLength(50)]
    public EntryTypeEnum EntryType { get; set; } = EntryTypeEnum.NormalEntry;
    public virtual Fund? Fund { get; set; }
    public virtual Daily? Daily { get; set; }
    public virtual List<FormDetails> FormDetails { get; set; } = new List<FormDetails>();
    // public virtual List<SubsidiaryJournal> SubsidiaryJournals { get; set; } = new List<SubsidiaryJournal>();
    public virtual Collage? Collage { get; set; }



}


