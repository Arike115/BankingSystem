using BankingSystem.Domain.Dto;

namespace BankingSystem.Interface
{
    public interface ITransactionRepository
    {
        Task<List<GetTransactionHistoryDto>> GetAllAccountTransaction(Guid Id);

    }
}
