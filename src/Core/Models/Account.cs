using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Models;

public class Account : BaseEntity
{
    [NotMapped]
    public override string? Name { get => base.Name; set => base.Name = value; }

    [StringLength(150)]
    public string AccountName { get; set; } = string.Empty;
    //[StringLength(50)]
    //public string AccountNumber { get; set; } = string.Empty;
    //دائن او مدين
    [StringLength(50)]
    public string AccountStatus { get; set; } = string.Empty;


    public List<SubAccount>? SubAccounts { get; set; }



}

//DbLC4iT-6QSTnCx