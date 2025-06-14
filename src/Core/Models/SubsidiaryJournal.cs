using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Models;

public class SubsidiaryJournal : BaseEntity
{
    [NotMapped]
    public override string? Name { get => base.Name; set => base.Name = value; }
    public int FormDetailsId { get; set; }
    public int SubAccountId { get; set; }
    public decimal? Debit { get; set; }
    public decimal? Credit { get; set; }



    public virtual SubAccount? SubAccount { get; set; }
    public virtual FormDetails? FormDetails { get; set; }


}


