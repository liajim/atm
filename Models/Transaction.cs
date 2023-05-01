using System.ComponentModel.DataAnnotations;

namespace AtmCoroBain.Models
{
    public class Transaction
    {
        [Key]
        public int Id { get; set; }
        public TransactionType TransactionType { get; set; }
        public DateTime TransactionDate { get; set; }
        public int AccountId { get; set; }
        public Account? Account { get; set; }
        public decimal Amount {get; set;}
        public int? WithDrawReferenceId { get; set; }
        public Transaction? WithDrawReference { get; set; }
    }
}
