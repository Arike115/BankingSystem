using BankingSystem.Domain.Model;
using MediatR;
using System.Text;

namespace BankingSystem.Application.Commands.UserRegistration
{
    public class UserRegistrationCommandHandler : IRequestHandler<UserRegistrationCommand, BaseResponse>
    {
        private readonly IMediator _mediator;
        private readonly ILogger<UserRegistrationCommandHandler> _logger;
        public UserRegistrationCommandHandler(IMediator mediator, ILogger<UserRegistrationCommandHandler> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        public async Task<BaseResponse> Handle(UserRegistrationCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var command = (RoleCommand)request;

                var response = await _mediator.Send(command, cancellationToken);
                return response;
            }
            catch (Exception e)
            {
                _logger.LogError($"An error occurred while creating applicant => {e.InnerException.Message} ||{e.InnerException.StackTrace}");
                return new BaseResponse(false, "An error occurred while creating applicant");
            }
        }

       
    }
 
}
