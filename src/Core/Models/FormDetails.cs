using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Models;

[Table("GeneralJournal")]
public class FormDetails : BaseEntity
{
    [NotMapped]
    public override string? Name { get => base.Name; set => base.Name = value; }
    public int FormId { get; set; }
    public int AccountId { get; set; }
    public decimal? Debit { get; set; }
    public decimal? Credit { get; set; }
    // [MaxLength(50)]
    // public string AccountType { get; set; } = string.Empty;
    public virtual Account? Account { get; set; }
    public virtual Form? Form { get; set; }
    public List<SubsidiaryJournal>? SubsidiaryJournals { get; set; }
}

