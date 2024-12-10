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
        /// <summary>
        /// handles the creation of transaction for the account
        /// the account number is the bankingsystem account user and the sender or receiver account and name is the where the user is sending
        /// money to or where the user is receiving the money from
        /// status : Debit is 1, credit is 2
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPost("create-transaction")]
        public async Task<IActionResult> CreateTransanction([FromBody] TransactionCommand command)
        {
            var response = await _mediator.Send(command);

            if (!response.Status)
                return BadRequest(response);

            return Ok(response);
        }

        /// <summary>
        /// this get the user transaction history
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [HttpGet("get-accountransactionhistory")]
        public async Task<IActionResult> GetJobDetails([FromQuery] GetTransactionHistoryQuery query)
        {
            var account = await _mediator.Send(query);
            return Ok(account);
        }
    }
}
