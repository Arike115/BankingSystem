using BankingSystem.Domain.Dto;
using BankingSystem.Domain.Model;
using MediatR;

namespace BankingSystem.Application.Query
{
    public class GetAccountDetailsQuery : IRequest<BaseResponse<GetAllAccountDto>>
    {
        public Guid AccountId { get; set; }
    }
}

