using BankingSystem.Domain.Dto;
using BankingSystem.Interface;

namespace BankingSystem.Services
{
    public class AccountRepository : IAccountRepository
    {
        public Task<List<GetAllAccountDto>> GetAllAccount()
        {
            throw new NotImplementedException();
        }
    }
}
