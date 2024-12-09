using BankingSystem.Domain.Dto;
using BankingSystem.Infrastructure.Persistence;
using BankingSystem.Interface;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;

namespace BankingSystem.Services
{
    public class AccountRepository : IAccountRepository
    {
        private readonly BankingSystemDbContext _context;

        public AccountRepository(BankingSystemDbContext context)
        {
            _context = context;
        }

        public async Task<List<GetAllAccountDto>> GetAllAccount()
        {
            // Fetch all accounts from the database and map to DTO
            var accounts = await _context.Account
                .Select(account => new GetAllAccountDto
                {
                    AccountId = account.Id,
                    AccountName = account.AccountName,
                    AccountNumber = account.AccountNumber,
                    Balance = account.AccountBalance,
                    CreatedOn = account.CreatedAt.DateTime
                }).ToListAsync();
            return accounts;
        }

        public async Task<GetAllAccountDto> GetAccountDetails(Guid accountId)
        {
            var account = await _context.Account
          .Where(a => a.Id == accountId)
          .Select(a => new GetAllAccountDto
          {
              AccountId = a.Id,
              AccountName = a.AccountName,
              AccountNumber = a.AccountNumber,
              Balance = a.AccountBalance,
              CreatedOn = a.CreatedAt.DateTime
          })
          .FirstOrDefaultAsync();

            return account;
        }
    }
}