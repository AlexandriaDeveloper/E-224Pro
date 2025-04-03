using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Models;

public class SubsidiaryJournal : BaseEntity
{
    [NotMapped]
    public override string? Name { get => base.Name; set => base.Name = value; }
    public int FormDetailsId { get; set; }
    public int SubAccountId { get; set; }
    public decimal? Amount { get; set; }

    public int? CollageId { get; set; }
    public int? FundId { get; set; }
    // Account  Credit or debit 
    //جانب المعامله مدين او دائن
    [MaxLength(10)]
    public string TransactionSide { get; set; } = "Credit";

    // PayRoll or General
    public string? AccountType { get; set; }
    //State Expensess or Special Funds
    public string? AccountItem { get; set; }
    public virtual SubAccount? SubAccount { get; set; }
    public virtual FormDetails? FormDetails { get; set; }
    public virtual Fund? Fund { get; set; }
    public virtual Collage? Collage { get; set; }

}


