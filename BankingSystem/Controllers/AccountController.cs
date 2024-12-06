using BankingSystem.Application.Commands.AccountRegistration;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace BankingSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : BaseController
    {
        private IMediator _mediator;
        public readonly ILogger<AccountController> _logger;
        public AccountController(IMediator mediator,ILogger<AccountController> logger)
        {
            _mediator = mediator ?? HttpContext.RequestServices.GetService<IMediator>();
            _logger = logger;
        }
        [HttpPost("create-account")]
        public async Task<ActionResult> CreateAccount([FromBody] AccountCommand command)
        {
            var response = await _mediator.Send(command);

            if (!response.Status)
                return BadRequest(response);

            return Ok(response);
        }
    }
}
