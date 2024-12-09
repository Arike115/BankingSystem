using BankingSystem.Application.Commands.AccountRegistration;
using BankingSystem.Application.Commands.TransactionRegistration;
using BankingSystem.Application.Query;
using BankingSystem.Domain.Dto;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace BankingSystem.Controllers
{
    public class TransactionController : BaseController
    {
        private IMediator _mediator;
        public readonly ILogger<AccountController> _logger;
        public TransactionController(IMediator mediator, ILogger<AccountController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }
        [HttpPost("create-transaction")]
        public async Task<ActionResult> CreateTransanction([FromBody] TransactionCommand command)
        {
            var response = await _mediator.Send(command);

            if (!response.Status)
                return BadRequest(response);

            return Ok(response);
        }
        [HttpGet("get-all-account")]
        public async Task<ActionResult<List<GetAllAccountDto>>> GetAllJob([FromQuery] GetAccountDetailsQuery query)
        {
            var account = await _mediator.Send(query);
            return Ok(account);
        }
    }
}
