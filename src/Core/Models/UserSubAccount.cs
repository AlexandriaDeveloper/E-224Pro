using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Models
{
    public class UserAccount
    {
        public string UserId { get; set; }
        public AppUser User { get; set; }

        public int AccountId { get; set; }
        public Account Account { get; set; }
    }
}
