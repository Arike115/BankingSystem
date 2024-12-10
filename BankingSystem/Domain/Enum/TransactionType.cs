namespace BankingSystem.Domain.Enum
{
    public enum TransactionType
    {
        Withdrawal = 1,
        Deposit,
        Transfer
    }

    public enum Status
    {

        Successful=1,
        pending,
        Failed,
        Reversed

    }

    public enum TransactionStatus
        {
            Debit =1,
            Credit = 2

        }
}


