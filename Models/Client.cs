using System.ComponentModel.DataAnnotations;

namespace AtmCoroBain.Models
{
    public class Client
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(30)]
        public string Name { get; set; }

        // One-to-many relationship with account
        public List<Account>? Accounts { get; set; }
    }
}
