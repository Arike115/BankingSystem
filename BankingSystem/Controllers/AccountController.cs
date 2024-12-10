using BankingSystem.Application.Commands.AccountRegistration;
using BankingSystem.Application.Query;
using BankingSystem.Domain.Dto;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BankingSystem.Controllers
{
    /// <summary>
    /// this controller is account 
    /// </summary>
   
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
        /// <summary>
        /// this is use for creating the the account for the user
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPost("create-account")]
        public async Task<IActionResult> CreateAccount([FromBody] AccountCommand command)
        {
            var response = await _mediator.Send(command);

            if (!response.Status)
                return BadRequest(response);

            return Ok(response);
        }
        /// <summary>
        /// this method get all the list of the account that has been created so far
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [HttpGet("get-all-account")]
        public async Task<IActionResult> GetAllJob([FromQuery] GetAccountQuery query)
        {
            var account = await _mediator.Send(query);
            return Ok(account);
        }
        /// <summary>
        /// this get the details of each account
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [HttpGet("get-accountdetails")]
        public async Task<IActionResult> GetJobDetails([FromQuery] GetAccountDetailsQuery query)
        {
            var account = await _mediator.Send(query);
            return Ok(account);
        }
    }
}
