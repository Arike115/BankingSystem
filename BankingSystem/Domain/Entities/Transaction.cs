using BankingSystem.Domain.Enum;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace BankingSystem.Domain.Entities
{
    public class Transaction : BaseEntity
    {
       
        public Guid AccountId { get; set; }
        public string AccountName { get; set; }

        public double Amount { get; set; }
        public string? Remarks {  get; set; }
        public DateTime TransactionDate { get; set; }
        public TransactionType TransactionType { get; set; }
        public bool IsDeleted { get; set; }
        public string? ReceiverAccountName {  get; set; }
        public long? ReceiverNumber {  get; set; }
        public string? SenderAccountName { get; set; }
        public long? SenderAccountNumber { get; set; }
        public string? BankName { get; set; }
        public Status Status { get; set; }
        public TransactionStatus Type { get; set; }

        public Account Account { get; set; }

    }
}
