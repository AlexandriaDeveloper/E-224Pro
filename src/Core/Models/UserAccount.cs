using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Models
{
    public class UserAccount : BaseEntity
    {
        [NotMapped]
        public override string? Name { get => base.Name; set => base.Name = value; }

        [NotMapped]
        public override int Id { get => base.Id; set => base.Id = value; }

        public string UserId { get; set; }
        public AppUser User { get; set; }

        public int AccountId { get; set; }
        public Account Account { get; set; }

    }
}
