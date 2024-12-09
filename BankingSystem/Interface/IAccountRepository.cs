using BankingSystem.Domain.Dto;
using BankingSystem.Domain.Entities;

namespace BankingSystem.Interface
{
    public interface IAccountRepository
    {
        Task<List<GetAllAccountDto>> GetAllAccount();

    }
}
