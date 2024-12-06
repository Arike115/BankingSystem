using BankingSystem.Domain.Enum;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace BankingSystem.Domain.Entities
{
    public class Account : BaseEntity
    {
        [Required]
        [MaxLength(100)]
        public string AccountName { get; set; }
        [Required]
        [MaxLength(50)]
        public string AccountNumber { get; set; }
        [Required]
        [MaxLength(50)]
        public string Bvn { get; set; }
        [Required]
        [MaxLength(50)]
        public string Nin { get; set; }
        public string Address {  get; set; }
        public double AccountBalance { get; set; }
        public AccountType AccountType { get; set; }
        public bool IsDeleted {  get; set; }

        public List<Transaction> Transactions { get; set; }
    }
}
