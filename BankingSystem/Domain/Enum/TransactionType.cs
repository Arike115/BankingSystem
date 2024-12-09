namespace BankingSystem.Domain.Enum
{
    public enum TransactionType
    {
        Withdrawal,
        Deposit,
        Transfer
    }

    public enum Status
    {

        Successful,
        pending,
        Failed,
        Reversed

    }

    public enum TransactionStatus
        {
            Debit,
            Credit

        }
}


