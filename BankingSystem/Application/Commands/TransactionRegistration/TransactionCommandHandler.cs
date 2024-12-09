﻿using BankingSystem.Application.Commands.AccountRegistration;
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

        public async Task<BaseResponse> Handle(TransactionCommand request, CancellationToken cancellationToken)
        {
            try
            { 
                var existingAccount = await _context.Account
               .FirstOrDefaultAsync(a =>
                   a.AccountName == request.AccountName, 
                   cancellationToken);
                if (existingAccount == null)
                {
                    throw new InvalidOperationException("An account with the same does not exists.");
                }

                if (request.Type == TransactionStatus.Debit && request.Amount > existingAccount.AccountBalance)
                { 
                    throw new InvalidOperationException("Insufficient fund.");
                }

                if(request.ReceiverAccountName.Count() != 10)
                {
                    throw new InvalidOperationException("Incomplete AccountNumber.");

                }
                var data = new Transaction();

                

                var command = new Account();
                command.AccountName = request.AccountName;
                command.AccountBalance = request.Amount;
                _context.Account.Update(command);
                await _context.SaveChangesAsync(cancellationToken);

                return new BaseResponse(true, "Account Created Successfully");
            }
            catch (Exception e)
            {
                _logger.LogError($"An error occurred while creating account => {e.InnerException.Message} || {e.InnerException.StackTrace}");
                return new BaseResponse(false, e.Message);
            }


        }
    }
}