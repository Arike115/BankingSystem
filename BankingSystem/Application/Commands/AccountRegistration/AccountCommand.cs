using BankingSystem.Domain.Enum;
using BankingSystem.Domain.Model;
using FluentValidation;
using MediatR;

namespace BankingSystem.Application.Commands.AccountRegistration
{
    /// <summary>
    /// handles the handler fr creating the Account for user
    /// </summary>
    public class AccountCommand : IRequest<BaseResponse>
    {
        public string AccountName { get; set; }
        public string AccountNumber { get; set; }
        public string Bvn { get; set; }
        public string Nin { get; set; }
        public string Address { get; set; }
        public double AccountBalance { get; set; }
        public AccountType AccountType { get; set; }
        public bool IsDeleted { get; set; }
    }

    /// <summary>
    /// handles the validation for the properties
    /// </summary>
    public class AccountCommandCommandValidator : AbstractValidator<AccountCommand>
    {

        public AccountCommandCommandValidator()
        {
            RuleFor(x => x.AccountName).NotEmpty().NotNull();
            RuleFor(x => x.Address).NotEmpty().NotNull();
            RuleFor(x => x.Bvn).NotEmpty().NotNull();
            RuleFor(x => x.Nin).NotEmpty().NotNull();
        }
    }
}
