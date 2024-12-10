using BankingSystem.Application.Commands.AccountRegistration;
using BankingSystem.Domain.Enum;
using BankingSystem.Domain.Model;
using FluentValidation;
using MediatR;

namespace BankingSystem.Application.Commands.TransactionRegistration
{
    /// <summary>
    /// this handles the transaction handler
    /// </summary>
    public class TransactionCommand : IRequest<BaseResponse>
    {
        public long AccountNumber { get; set; }
        public double Amount { get; set; }
        public string? Remarks { get; set; }
        public DateTime TransactionDate { get; set; }
        public string? ReceiverAccountName { get; set; }
        public long? ReceiverNumber { get; set; }
        public string? SenderAccountName { get; set; }
        public long? SenderAccountNumber { get; set; }
        public string? BankName { get; set; }
        public TransactionStatus Type { get; set; }

    }
    /// <summary>
    /// this handles the neccesary validation for the transaction command
    /// </summary>
    public class TransactionCommandCommandValidator : AbstractValidator<TransactionCommand>
    {

        public TransactionCommandCommandValidator()
        {
            RuleFor(x => x.AccountNumber).NotEmpty().Equals(10);
            RuleFor(x => x.BankName).NotEmpty().NotNull();
            RuleFor(x => x.Amount).NotEmpty().NotNull();
        }
    }
}
