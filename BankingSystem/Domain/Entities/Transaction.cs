using BankingSystem.Domain.Enum;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace BankingSystem.Domain.Entities
{
    public class Transaction : BaseEntity
    {
       
        public Guid AccountId { get; set; }
        public decimal Amount { get; set; }
        public string Type { get; set; }    
        public string Remarks {  get; set; }
        public DateTime TransactionDate { get; set; }
        public TransactionType TransactionType { get; set; }
        public bool IsDeleted { get; set; }
        public string Sender {  get; set; }
        public string AccountName {  get; set; }
        public string Receiver {  get; set; }

        public Account Account { get; set; }

    }
}
