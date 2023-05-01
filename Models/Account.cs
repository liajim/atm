using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AtmCoroBain.Models
{
    public class Account
    {
        [Key]
        public int Id { get; set; }
        public int ClientId { get; set; }
        public Client? Client { get; set; }

        // One-to-many relationship with transaction
        public List<Transaction>? Transactions { get; set; }
        
        [NotMapped]
        public decimal Balance { get; set; }

    }
}
