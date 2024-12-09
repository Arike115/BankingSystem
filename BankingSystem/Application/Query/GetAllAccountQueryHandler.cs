using AutoMapper;
using BankingSystem.Domain.Dto;
using BankingSystem.Domain.Model;
using BankingSystem.Infrastructure.Persistence.Extensions;
using BankingSystem.Interface;
using Hangfire.Common;
using MediatR;
using System.Globalization;

namespace BankingSystem.Application.Query
{
    public class GetAllAccountQueryHandler : IRequestHandler<GetAccountQuery, BaseResponse<List<GetAllAccountDto>>>
    {
        public readonly ILogger<GetAllAccountQueryHandler> _logger;
        private readonly IMapper _mapper;
        private readonly IAccountRepository _accountRepository;

        public GetAllAccountQueryHandler(ILogger<GetAllAccountQueryHandler> logger, IMapper mapper, IAccountRepository accountRepository)
        {
            _logger = logger;
            _mapper = mapper;
            _accountRepository = accountRepository;

        }
        public async Task<BaseResponse<List<GetAllAccountDto>>> Handle(GetAccountQuery request, CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInformation("GetAllAccountQueryHandler entry");

                //  Retrieve all accounts
                var accounts = await _accountRepository.GetAllAccount();

                if (accounts == null || !accounts.Any())
                {
                    return new BaseResponse<List<GetAllAccountDto>>
                    {
                        Status = true,
                        Message = "No accounts found.",
                        Data = new List<GetAllAccountDto>()
                    };
                }

                // Apply Filters (optional)
                if (!string.IsNullOrEmpty(request.AccountName))
                {
                    accounts = accounts.Where(a => a.AccountName.Contains(request.AccountName)).ToList();
                }

                if (!string.IsNullOrEmpty(request.AccountNumber))
                {
                    accounts = accounts.Where(a => a.AccountNumber.ToString().Contains(request.AccountNumber)).ToList();
                }

                // Pagination
                var pages = PagingExtensions.PagedResult(accounts, request.pageSize);

                var pagedResult = accounts.Page(request.pageNumber, request.pageSize);

                // Map to Getallaccountdto
                var accountDtos = _mapper.Map<List<GetAllAccountDto>>(pagedResult);

                // Count recent accounts created in the last 24 hours
                var recentCount = accounts.Count(a => a.CreatedOn > DateTime.Now.AddHours(-24));

                //Return response
                return new BaseResponse<List<GetAllAccountDto>>
                {
                    Status = true,
                    Message = "Accounts retrieved successfully.",
                    Data = accountDtos,
                    PageNumber = request.pageNumber,
                    PageSize = request.pageSize,
                    TotalRecords = accounts.Count,
                    TotalPages = (int)Math.Ceiling((double)accounts.Count / request.pageSize),
                    
                };
            }
            catch (Exception e)
            {
                if (e.InnerException != null)
                {
                    _logger.LogError($"An error occurred while retrieving account types => {e.InnerException.Message} || {e.InnerException.StackTrace}");
                }
                _logger.LogError($"An error occurred while retrieving account types => {e.Message} || {e.StackTrace}");

                return new BaseResponse<List<GetAllAccountDto>>();
            }
        }
    }
}
