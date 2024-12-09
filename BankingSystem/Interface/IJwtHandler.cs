using BankingSystem.Domain.Model;
using System.Security.Claims;

namespace BankingSystem.Interface
{
    public interface IJwtHandler
    {
        SimpleJsonWebToken CreateSimple(string userId, string email, List<Claim> roleClaims);
        JsonWebToken Create(string userId, string email, string fullName, string BusinessName,
            string PhoneNumber, List<Claim> roleClaims, bool islogin);
        ClaimsPrincipal GetPrincipalFromToken(string token);
    }
}
