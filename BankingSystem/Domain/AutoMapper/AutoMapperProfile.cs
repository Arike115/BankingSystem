using AutoMapper;
using BankingSystem.Domain.Dto;
using BankingSystem.Domain.Entities;
using Hangfire.Common;

namespace BankingSystem.Domain.AutoMapper
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Account, GetAllAccountDto>();
        }
    }
}
