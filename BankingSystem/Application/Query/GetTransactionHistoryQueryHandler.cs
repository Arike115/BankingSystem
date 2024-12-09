using AutoMapper;
using BankingSystem.Domain.Dto;
using BankingSystem.Domain.Model;
using BankingSystem.Infrastructure.Persistence.Extensions;
using BankingSystem.Interface;
using BankingSystem.Services;
using MediatR;

namespace BankingSystem.Application.Query
{
    public class GetTransactionHistoryQueryHandler : IRequestHandler<GetTransactionHistoryQuery, BaseResponse<List<GetTransactionHistoryDto>>>
    {
        public readonly ILogger<GetTransactionHistoryQueryHandler> _logger;
        private readonly IMapper _mapper;
        private readonly ITransactionRepository _transRepository;

        public GetTransactionHistoryQueryHandler(ILogger<GetTransactionHistoryQueryHandler> logger, IMapper mapper, ITransactionRepository transRepository)
        {
            _logger = logger;
            _mapper = mapper;
            _transRepository = transRepository;

        }
        public async Task<BaseResponse<List<GetTransactionHistoryDto>>> Handle(GetTransactionHistoryQuery request, CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInformation("GetAllAccountQueryHandler entry");

                //  Retrieve all transactions for a particular account
                var data = await _transRepository.GetAllAccountTransaction(request.AccountId);

                if (data == null || !data.Any())
                {
                    return new BaseResponse<List<GetTransactionHistoryDto>>
                    {
                        Status = true,
                        Message = "No accounts found.",
                        Data = new List<GetTransactionHistoryDto>()
                    };
                }


                // Pagination
                var pages = PagingExtensions.PagedResult(data, request.pageSize);

                var pagedResult = data.Page(request.pageNumber, request.pageSize);

                // Map to GetTransactionHistoryDto
                var accountDtos = _mapper.Map<List<GetTransactionHistoryDto>>(pagedResult);

                //Return response
                return new BaseResponse<List<GetTransactionHistoryDto>>
                {
                    Status = true,
                    Message = "Transaction retrieved successfully.",
                    Data = accountDtos,
                    PageNumber = request.pageNumber,
                    PageSize = request.pageSize,
                    TotalRecords = data.Count,
                    TotalPages = (int)Math.Ceiling((double)data.Count / request.pageSize),

                };
            }
            catch (Exception e)
            {
                if (e.InnerException != null)
                {
                    _logger.LogError($"An error occurred while retrieving Transaction types => {e.Message} || {e.StackTrace}");
                }
                _logger.LogError($"An error occurred while Transaction job types => {e.Message} || {e.StackTrace}");

                return new BaseResponse<List<GetTransactionHistoryDto>>();
            }
        }
    }
}
