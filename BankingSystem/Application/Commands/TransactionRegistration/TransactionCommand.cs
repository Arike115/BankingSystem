using BankingSystem.Application.Commands.AccountRegistration;
using BankingSystem.Domain.Enum;
using BankingSystem.Domain.Model;
using FluentValidation;
using MediatR;

namespace BankingSystem.Application.Commands.TransactionRegistration
{
    public class TransactionCommand : IRequest<BaseResponse>
    {
        public Guid AccountId { get; set; }
        public string AccountName { get; set; }
        public double Amount { get; set; }
        public string? Remarks { get; set; }
        public DateTime TransactionDate { get; set; }
        public TransactionType TransactionType { get; set; }
        public bool IsDeleted { get; set; }
        public string? ReceiverAccountName { get; set; }
        public long? ReceiverNumber { get; set; }
        public string? SenderAccountName { get; set; }
        public long? SenderAccountNumber { get; set; }
        public string? BankName { get; set; }
        public Status Status { get; set; }
        public TransactionStatus Type { get; set; }

    }

    public class TransactionCommandCommandValidator : AbstractValidator<TransactionCommand>
    {

        public TransactionCommandCommandValidator()
        {
            RuleFor(x => x.AccountName).NotEmpty().NotNull();
            RuleFor(x => x.BankName).NotEmpty().NotNull();
            RuleFor(x => x.Amount).NotEmpty().NotNull();
            RuleFor(x => x.Status).NotEmpty().NotNull();
        }
    }
}
