using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Models;

public class AuditLog : BaseEntity
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public override int Id { get; set; }
    [NotMapped]
    public override string? Name { get => base.Name; set => base.Name = value; }
    [MaxLength(50)]
    public string UserId { get; set; } = string.Empty;
    [MaxLength(50)]
    public string EntityName { get; set; } = string.Empty;
    [MaxLength(50)]
    public string Action { get; set; } = string.Empty;
    [MaxLength(5000)]
    public string? OldValue { get; set; }
    [MaxLength(5000)]
    public string? NewValue { get; set; } = string.Empty;
    // [MaxLength(500)]
    // public string Details { get; set; } = string.Empty;
    [MaxLength(500)]
    public string? AffectedColumns { get; set; }
    [MaxLength(100)]
    public string? PrimaryKey { get; set; } = string.Empty;
    public DateTime Timestamp { get; set; }




}


