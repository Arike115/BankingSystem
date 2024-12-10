using BankingSystem.Application.Commands.UserRegistration;
using BankingSystem.Domain.Model;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace BankingSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : BaseController
    {
        private IMediator _mediator;
        public readonly ILogger<AccountController> _logger;
        public AuthController(IMediator mediator, ILogger<AccountController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }
        /// <summary>
        /// Logs in or authenticate a user and returns a JWT token object.
        /// </summary>
        /// <param name="model"></param>
        /// <response code="200">User credential is correct and token is returnedy</response>
        /// <response code="400">If user credential is wrong</response>
        [Produces("application/json")]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(BaseResponse<JsonWebToken>), (int)HttpStatusCode.OK)]
        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] LoginCommand model)
        {
            var response = await _mediator.Send(model);
            if (!response.Status)
                return BadRequest(response);

            return Ok(response);
        }
    }
}
