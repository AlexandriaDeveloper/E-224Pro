using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace Core.Models;

public class Fund : BaseEntity
{
    [NotMapped]
    public override string? Name { get; set; }
    [MaxLength(100)]
    public string FundName { get; set; } = string.Empty;
    public string FundCode { get; set; } = string.Empty;
    public int CollageId { get; set; }
    public List<Form> Forms { get; set; } = new List<Form>();
}


