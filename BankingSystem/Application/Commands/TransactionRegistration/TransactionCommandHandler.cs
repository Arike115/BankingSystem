using BankingSystem.Application.Commands.AccountRegistration;
using BankingSystem.Domain.Entities;
using BankingSystem.Domain.Enum;
using BankingSystem.Domain.Model;
using BankingSystem.Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BankingSystem.Application.Commands.TransactionRegistration
{
    public class TransactionCommandHandler : IRequestHandler<TransactionCommand, BaseResponse>
    {
        private readonly MediatR.IMediator _mediator;
        private readonly ILogger<TransactionCommandHandler> _logger;
        private readonly IWebHostEnvironment _env;
        private readonly BankingSystemDbContext _context;
        public TransactionCommandHandler(MediatR.IMediator mediator, ILogger<TransactionCommandHandler> logger, BankingSystemDbContext context, IWebHostEnvironment env)
        {
            _mediator = mediator;
            _logger = logger;
            _env = env;
            _context = context;
        }
        /// <summary>
        /// this method handles the creation of the transaction , i.e thw flow of how the debit and the credit works 
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        public async Task<BaseResponse> Handle(TransactionCommand request, CancellationToken cancellationToken)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(request.SenderAccountName) || !string.IsNullOrWhiteSpace(request.ReceiverAccountName))
                {
                    return new BaseResponse(false, "Beneficiary name cannot be empty");

                }
                if (request.AccountNumber == 0)
                {
                    return new BaseResponse(false, "account number cannot be empty");

                }
                if (request.SenderAccountNumber == 0 || request.ReceiverNumber == 0)
                {
                    return new BaseResponse(false, "beneficiary number cannot be empty");

                }
                var existingAccount = await _context.Account
               .FirstOrDefaultAsync(a =>
                   a.AccountNumber == request.AccountNumber, 
                   cancellationToken);
                if (existingAccount == null)
                {
                    throw new InvalidOperationException("An account with the account  number does not exists.");
                }

                if (request.Type == TransactionStatus.Debit && request.Amount > existingAccount.AccountBalance)
                { 
                    throw new InvalidOperationException("Insufficient fund.");
                }
                
                if(request.Type == TransactionStatus.Debit && request.ReceiverAccountName !=null && request.ReceiverAccountName.Count() != 10)
                {
                    throw new InvalidOperationException("Incomplete AccountNumber.");

                }
                using var transaction = await _context.Database.BeginTransactionAsync(cancellationToken);

                try
                {
                    // Deduct from sender
                    if ((int)request.Type == (int)TransactionStatus.Debit)
                    {
                        existingAccount.AccountBalance -= request.Amount;
                    }

                    // Add to receiver
                    if ((int)request.Type == (int)TransactionStatus.Credit)
                    {
                        existingAccount.AccountBalance += request.Amount;
                    }
                    // Save transaction entry (audit log)
                    var data = new Transaction();
                    data.AccountId = existingAccount.Id;
                    data.AccountName = existingAccount.AccountName;
                    data.Amount = request.Amount;
                    data.Remarks = request.Remarks;
                    data.TransactionDate = request.TransactionDate;
                    data.TransactionType = TransactionType.Transfer;
                    data.ReceiverAccountName = request.ReceiverAccountName;
                    data.ReceiverNumber = request.ReceiverNumber;
                    data.SenderAccountName = request.SenderAccountName;
                    data.SenderAccountNumber = request.SenderAccountNumber;
                    data.BankName = request.BankName;
                    data.Status = Status.Successful;
                    data.Type = request.Type;
                    data.CreatedBy = existingAccount.AccountName;
                    await _context.Transaction.AddAsync(data, cancellationToken);

                    // Save changes
                    await _context.SaveChangesAsync(cancellationToken);

                    // Commit transaction
                    _context.Entry(existingAccount).State = EntityState.Modified;

                    await transaction.CommitAsync(cancellationToken);

                    return new BaseResponse(true, "transaction Created Successfully");
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync(cancellationToken);
                    return new BaseResponse(false, ex.Message);
                }
            }
            catch (Exception e)
            {
                _logger.LogError($"An error occurred while creating account => {e.Message} || {e.StackTrace}");
                return new BaseResponse(false, e.Message);
            }


        }
    }
}
