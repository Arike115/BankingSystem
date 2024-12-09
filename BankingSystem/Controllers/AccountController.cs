using BankingSystem.Application.Commands.AccountRegistration;
using BankingSystem.Application.Query;
using BankingSystem.Domain.Dto;
using MediatR;
using Microsoft.AspNetCore.Authorization;
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
        //[Authorize]
        [HttpGet("get-all-account")]
        public async Task<ActionResult<List<GetAllAccountDto>>> GetAllJob([FromQuery] GetAccountQuery query)
        {
            var account = await _mediator.Send(query);
            return Ok(account);
        }

        [HttpGet("get-accountdetails")]
        public async Task<ActionResult<List<GetAllAccountDto>>> GetJobDetails([FromQuery] GetAccountDetailsQuery query)
        {
            var account = await _mediator.Send(query);
            return Ok(account);
        }
    }
}
