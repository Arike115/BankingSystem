using BankingSystem.Domain.Dto;
using BankingSystem.Domain.Model;
using MediatR;

namespace BankingSystem.Application.Query
{
    public class GetTransactionHistoryQuery : IRequest<BaseResponse<List<GetTransactionHistoryDto>>>
    {
        public Guid AccountId { get; set; }
        public int pageNumber { get; set; } = 1;

        public int pageSize { get; set; } = 10;

    }
}
