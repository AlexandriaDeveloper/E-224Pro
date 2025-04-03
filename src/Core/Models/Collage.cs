using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace Core.Models;


public class Collage : BaseEntity
{
    [NotMapped]
    public override string Name { get; set; }
    [MaxLength(100)]
    public string CollageName { get; set; } = string.Empty;

}