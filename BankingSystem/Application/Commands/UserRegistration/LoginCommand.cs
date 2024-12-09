using BankingSystem.Domain.Model;
using MediatR;

namespace BankingSystem.Application.Commands.UserRegistration
{
    public class LoginCommand : IRequest<BaseResponse<JsonWebToken>>
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
