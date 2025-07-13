using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Models;

public class BaseEntity
{
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    public virtual int Id { get; set; }
    // [MaxLength(50)]
    // public virtual Guid? TempId { get; set; } = Guid.NewGuid();
    [MaxLength(100)]
    public virtual string? Name { get; set; }

    public DateTime CreatedDate { get; set; }
    [MaxLength(100)]
    public string CreatedBy { get; set; } = string.Empty;


}
