using BankingSystem.Domain.Model;
using MediatR;

namespace BankingSystem.Application.Commands.UserRegistration
{
    public class RoleCommand : IRequest<BaseResponse>
    {
        public string BusinessName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Industry { get; set; }
        public string Location { get; set; }
        public string Password { get; set; }
       // public UserTypeEnum UserType { get; set; }
        public string SecurityAnswer { get; set; }


    }
}
