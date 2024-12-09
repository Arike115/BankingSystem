using BankingSystem.Domain.Entities;
using BankingSystem.Domain.Model;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json;

namespace BankingSystem.Application.Commands.UserRegistration
{
   
    public class RoleommandHandler : IRequestHandler<RoleCommand, BaseResponse>
    {
        private readonly MediatR.IMediator _mediator;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<RoleommandHandler> _logger;
        private readonly IWebHostEnvironment _env;

        public RoleommandHandler(MediatR.IMediator mediator, UserManager<ApplicationUser> userManager, ILogger<RoleommandHandler> logger, IWebHostEnvironment env)
        {
            _mediator = mediator;
            _userManager = userManager;
            _logger = logger;
            _env = env;
        }

        public async Task<BaseResponse> Handle(RoleCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var email = request.Email.Trim().ToLowerInvariant();

                var existingUser = await _userManager.FindByEmailAsync(email);

                if (existingUser != null)
                {
                    return new BaseResponse(false, "A user with this email already exists. If this is you, please log in. .");
                }

                var newUser = (ApplicationUser)request;

                _logger.LogInformation($"CREATE USER PAYLOAD FOR {email} : {JsonConvert.SerializeObject(newUser)}");

                var registerAttempt = await _userManager.CreateAsync(newUser, request.Password);
                if (!registerAttempt.Succeeded)
                {
                    var errors = registerAttempt.Errors.Select(x => x.Description).ToList();
                    return new BaseResponse(false, $"We could not complete your registration. {errors.FirstOrDefault()}.");
                }

                //check if the env is staging or QA, return a test token with the user Id else, send an email
                //if(_env.)
                //send registration sucess to the user
                //var inform = await _mediator.Send(new SendEmailConfirmationCommand { User = newUser }, cancellationToken);


                return new BaseResponse(true, "Registration successful.");
                // return new BaseResponse(true, "Registration successful.",inform.Messages);
            }
            catch (Exception e)
            {
                _logger.LogError($"An error occurred => {e.Message}");
            }
            return new BaseResponse(false, "An error occurred");

        }
    }
}
