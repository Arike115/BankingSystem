using BankingSystem.Domain.Enum;

namespace BankingSystem.Domain.Dto
{
    public class GetTransactionHistoryDto
    {
        public double Amount { get; set; }
        public DateTime TransactionDate { get; set; }
        public TransactionType TransactionType { get; set; }
        public string? BeneficiaryAccountName { get; set; }
        public long? BeneficiarAccountNumber { get; set; }
        public string? BankName { get; set; }
        public Status Status { get; set; }
        public TransactionStatus Type { get; set; }
    }
}
