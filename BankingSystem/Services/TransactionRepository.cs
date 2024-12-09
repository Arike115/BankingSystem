using BankingSystem.Domain.Dto;
using BankingSystem.Infrastructure.Persistence;
using BankingSystem.Interface;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using System.Diagnostics;

namespace BankingSystem.Services
{
    public class TransactionRepository : ITransactionRepository
    {
        private readonly BankingSystemDbContext _context;

        public TransactionRepository(BankingSystemDbContext context)
        {
            _context = context;
        }
        public async Task<List<GetTransactionHistoryDto>> GetAllAccountTransaction(Guid Id)
        {
            var transact = await _context.Transaction
         .Where(a => a.AccountId == Id)
         .Select(a => new GetTransactionHistoryDto
         {
             BeneficiarAccountNumber = a.SenderAccountNumber == 0 ? a.ReceiverNumber : a.SenderAccountNumber,
             BeneficiaryAccountName = a.SenderAccountName == null ? a.ReceiverAccountName : a.SenderAccountName,
             Amount = a.Amount,
             TransactionDate = a.CreatedAt.DateTime
         }).ToListAsync();

            return transact;
        }
    }
}
