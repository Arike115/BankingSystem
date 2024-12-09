using AutoMapper;
using BankingSystem.Domain.Dto;
using BankingSystem.Domain.Model;
using BankingSystem.Infrastructure.Persistence.Extensions;
using BankingSystem.Interface;
using MediatR;

namespace BankingSystem.Application.Query
{
    public class GetAccountDetailsQueryHandler : IRequestHandler<GetAccountDetailsQuery, BaseResponse<GetAllAccountDto>>
    {
        public readonly ILogger<GetAccountDetailsQueryHandler> _logger;
        private readonly IMapper _mapper;
        private readonly IAccountRepository _accountRepository;

        public GetAccountDetailsQueryHandler(ILogger<GetAccountDetailsQueryHandler> logger, IMapper mapper, IAccountRepository accountRepository)
        {
            _logger = logger;
            _mapper = mapper;
            _accountRepository = accountRepository;

        }

        public async Task<BaseResponse<GetAllAccountDto>> Handle(GetAccountDetailsQuery request, CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInformation("GetAllAccountQueryHandler entry");

                //  Retrieve all accounts
                var accounts = await _accountRepository.GetAccountDetails(request.AccountId);

                //Return response
                return new BaseResponse<GetAllAccountDto>
                {
                    Status = true,
                    Message = "Accounts retrieved successfully.",
                    Data = accounts,

                };
            }
            catch (Exception e)
            {
                _logger.LogError($"An error occurred while retrieving job types => {e.Message} || {e.StackTrace}");

                return new BaseResponse<GetAllAccountDto>();
            }
        }
    }
   
}
