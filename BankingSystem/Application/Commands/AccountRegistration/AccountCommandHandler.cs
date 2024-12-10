using BankingSystem.Domain.Entities;
using BankingSystem.Domain.Enum;
using BankingSystem.Domain.Model;
using BankingSystem.Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Text;

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

        /// <summary>
        /// this method create account for user and it also auto generate the account number with neccessary checks
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        public async Task<BaseResponse> Handle(AccountCommand request, CancellationToken cancellationToken)
        {
            try
            {

                if (!string.IsNullOrWhiteSpace(request.AccountName))
                {
                    return new BaseResponse(false, "Account Name canot be empty");

                }
                if (!string.IsNullOrWhiteSpace(request.AccountNumber))
                {
                    return new BaseResponse(false, "account number cannot be empty");
                }
                if (!string.IsNullOrWhiteSpace(request.Bvn))
                {
                    return new BaseResponse(false, "bvn is required");
                }
                if (!string.IsNullOrWhiteSpace(request.Nin))
                {
                    return new BaseResponse(false, "nin is required");
                }
                var existingAccount = await _context.Account
             .FirstOrDefaultAsync(a =>
                 a.AccountName == request.AccountName ||
                 a.Bvn == request.Bvn ||
                 a.Nin == request.Nin,
                 cancellationToken);

                if (existingAccount != null)
                {
                    return new BaseResponse(false, "An account with the same details already exists.");
                }
                var command = new Account();
                command.AccountName = request.AccountName;
                command.Bvn = request.Bvn;
                command.Nin = request.Nin;
                command.AccountNumber = long.Parse(GenerateRandomStringNumbersOnly(10)); 
                command.AccountType = AccountType.Individual;
                command.Address = request.Address;
                command.CreatedBy = "Admin";
                command.CreatedAt = DateTime.UtcNow;

                _context.Account.Add(command);
                await _context.SaveChangesAsync(cancellationToken);

                return new BaseResponse(true, "Account Created Successfully");
            }
            catch (Exception e)
            {
                _logger.LogError($"An error occurred while creating account => {e.Message} || {e.StackTrace}");
                return new BaseResponse(false, e.Message);
            }
        }

        /// <summary>
        /// this method is used for generating the random account number
        /// </summary>
        /// <param name="length"></param>
        /// <returns></returns>
        public static string GenerateRandomStringNumbersOnly(int length)
        {
            string randomstring;

            StringBuilder oBuilder = new StringBuilder();
            while (oBuilder.Length <= length)
            {
                Guid guid = Guid.NewGuid();
                oBuilder.Append(guid.ToString("N").ToUpperInvariant());
            }
            randomstring = oBuilder.ToString();

            byte[] asciiBytes = Encoding.ASCII.GetBytes(randomstring);
            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < asciiBytes.Length; i++)
            {
                builder.Append(asciiBytes[i].ToString());
            }

            return builder.ToString(0, length);
        }
    }

}
