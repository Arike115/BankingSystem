using BankingSystem.Domain.Entities;
using BankingSystem.Domain.Enum;
using BankingSystem.Domain.Model;
using BankingSystem.Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace BankingSystem.Application.Commands.AccountRegistration
{
    public class AccountCommandHandler : IRequestHandler<AccountCommand, BaseResponse>
    {
        private readonly MediatR.IMediator _mediator;
        private readonly ILogger<AccountCommandHandler> _logger;
        private readonly IWebHostEnvironment _env;
        private readonly BankingSystemDbContext _context;


        public AccountCommandHandler(MediatR.IMediator mediator,ILogger<AccountCommandHandler> logger, BankingSystemDbContext context ,IWebHostEnvironment env)
        {
            _mediator = mediator;
            _logger = logger;
            _env = env;
            _context = context;
        }

        public async Task<BaseResponse> Handle(AccountCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var existingAccount = await _context.Account
             .FirstOrDefaultAsync(a =>
                 a.AccountName == request.AccountName ||
                 a.Bvn == request.Bvn ||
                 a.Nin == request.Nin,
                 cancellationToken);

                if (existingAccount != null)
                {
                    throw new InvalidOperationException("An account with the same details already exists.");
                }
                var command = new Account();
                command.AccountName = request.AccountName;
                command.Bvn = request.Bvn;
                command.Nin = request.Nin;
                command.AccountNumber = request.AccountNumber;
                command.AccountType = AccountType.Individual;
                command.Address = request.Address;

                _context.Account.Add(command);
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
