using BankingSystem.Domain.Model;
using FluentValidation;
using MediatR;

namespace BankingSystem.Application.Commands.UserRegistration
{
   
        public class UserRegistrationCommand : IRequest<BaseResponse>
        {
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public string Email { get; set; }
            public string PhoneNumber { get; set; }
            public string Password { get; set; }
            public string Address { get; set; }

            public static explicit operator RoleCommand(UserRegistrationCommand source)
            {
                var destination = new RoleCommand();
                destination.FirstName = source.FirstName;
                destination.LastName = source.LastName;
                destination.Email = source.Email.Trim().ToLowerInvariant();
                destination.PhoneNumber = source.PhoneNumber;
                destination.Password = source.Password;
                //destination.UserType = Domain.Enums.UserTypeEnum.Applicant;
                return destination;
            }

        }

        public class UserRegistrationCommandValidator : AbstractValidator<UserRegistrationCommand>
        {
            public UserRegistrationCommandValidator()
            {
                RuleFor(x => x.Email).NotEmpty().NotNull().EmailAddress();
                RuleFor(x => x.Password).NotEmpty().NotNull().MinimumLength(4);
                RuleFor(x => x.FirstName).NotEmpty().NotNull();
                RuleFor(x => x.LastName).NotEmpty().NotNull();
                RuleFor(x => x.PhoneNumber).NotEmpty().NotNull();
            }
        }
    
}
