using BankingSystem.Domain.Dto;
using BankingSystem.Domain.Model;
using MediatR;
using System.Globalization;

namespace BankingSystem.Application.Query
{
    public class GetAccountQuery : IRequest<BaseResponse<List<GetAllAccountDto>>>
    {

        // public Sorted sortedBy { get; set; }
        public string AccountName { get; set; }
        public string AccountNumber { get; set; }
        public int pageNumber { get; set; } = 1;

        public int pageSize { get; set; } = 10;


    }
}
