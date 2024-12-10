using BankingSystem.Domain.Model;
using MediatR;

namespace BankingSystem.Application.Commands.UserRegistration
{
    /// <summary>
    /// this command is useed for login purpose it handles the loginhandler
    /// </summary>
    public class LoginCommand : IRequest<BaseResponse<JsonWebToken>>
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
