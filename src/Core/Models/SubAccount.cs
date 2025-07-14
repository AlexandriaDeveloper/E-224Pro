using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Models;

public class SubAccount : BaseEntity
{

    [NotMapped]
    public override string? Name { get => base.Name; set => base.Name = value; }
    public int AccountId { get; set; }

    [MaxLength(150)]
    public string SubAccountName { get; set; } = string.Empty;
    // [StringLength(50)]
    // public string SubAccountNumber { get; set; } = string.Empty;
    public Account? Account { get; set; }
}

