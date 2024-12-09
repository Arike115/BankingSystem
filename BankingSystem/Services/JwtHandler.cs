using BankingSystem.Domain.Model;
using BankingSystem.Interface;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace BankingSystem.Services
{
    public class JwtHandler : IJwtHandler
    {
        private readonly JwtSecurityTokenHandler _jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
        private readonly IConfiguration Configuration;
        private readonly SecurityKey _issuerSigningKey;
        private readonly SigningCredentials _signingCredentials;
        private readonly JwtHeader _jwtHeader;
        private readonly TokenValidationParameters _tokenValidationParameters;
        private readonly ILogger<JwtHandler> _logger;

        public JwtHandler(IConfiguration configuration, TokenValidationParameters tokenValidationParameters,
        ILogger<JwtHandler> logger)
        {
            Configuration = configuration;
            _issuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(Configuration["Jwt:SigningKey"]));
            _signingCredentials = new SigningCredentials(_issuerSigningKey, SecurityAlgorithms.HmacSha256);
            _jwtHeader = new JwtHeader(_signingCredentials);
            _tokenValidationParameters = tokenValidationParameters;
            _logger = logger;
        }

        public JsonWebToken Create(string userId, string email, string fullName, string BusinessName, 
            string PhoneNumber,  List<Claim> roleClaims, bool islogin)
        {
            var nowUtc = DateTime.UtcNow;
            var expires = nowUtc.AddMinutes(Configuration.GetValue<int>("Jwt:ExpiryInMinutes"));
            var centuryBegins = new DateTime(1970, 1, 1).ToUniversalTime();
            var exp = (long)new TimeSpan(expires.Ticks - centuryBegins.Ticks).TotalSeconds;
            var now = (long)new TimeSpan(nowUtc.Ticks - centuryBegins.Ticks).TotalSeconds;

            var payload = new JwtPayload
            {
                {"sub", userId},
                {"iss", Configuration.GetValue<string>("Jwt:Issuer")},
                {"iat", now},
                {"exp", exp},
                {"unique_name", email},
                {"islogin",islogin},
                {"user_Id", userId},
                {System.IdentityModel.Tokens.Jwt.JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()}

            };

            if (roleClaims != null && roleClaims.Count > 0)
                payload.AddClaims(roleClaims);


            var jwt = new JwtSecurityToken(_jwtHeader, payload);
            var token = _jwtSecurityTokenHandler.WriteToken(jwt);



            return new JsonWebToken
            {
                Token = token,
                Expires = exp,
                FullName = fullName,
                ProfileId = userId,
                Email = email,
                Islogin = islogin,


            };
        }
        public SimpleJsonWebToken CreateSimple(string userId, string email, List<Claim> roleClaims)
        {
            var nowUtc = DateTime.UtcNow;
            var expires = nowUtc.AddMinutes(Configuration.GetValue<int>("Jwt:ExpiryInMinutes"));
            var centuryBegins = new DateTime(1970, 1, 1).ToUniversalTime();
            var exp = (long)new TimeSpan(expires.Ticks - centuryBegins.Ticks).TotalSeconds;
            var now = (long)new TimeSpan(nowUtc.Ticks - centuryBegins.Ticks).TotalSeconds;

            var payload = new JwtPayload
            {
                {"sub", userId},
                {"iss", Configuration.GetValue<string>("Jwt:Issuer")},
                {"iat", now},
                {"exp", exp},
                {"unique_name", email},
            };

            if (roleClaims != null && roleClaims.Count > 0)
                payload.AddClaims(roleClaims);


            var jwt = new JwtSecurityToken(_jwtHeader, payload);
            var token = _jwtSecurityTokenHandler.WriteToken(jwt);


            return new SimpleJsonWebToken
            {
                Token = token,
                Expires = exp,
                Email = email,
                UserId = userId
            };
        }

        public ClaimsPrincipal GetPrincipalFromToken(string token)
        {
            try
            {
                _tokenValidationParameters.ValidateLifetime = false;
                var principal = _jwtSecurityTokenHandler.ValidateToken(token, _tokenValidationParameters, out SecurityToken validatedToken);
                if (!IsJwtWithValidSecurityAlgorithm(validatedToken))
                {
                    return null;
                }
                _tokenValidationParameters.ValidateLifetime = true;

                return principal;

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while getting principal from token");
                return null;
            }
        }

        private bool IsJwtWithValidSecurityAlgorithm(SecurityToken validatedToken)
        {
            return (validatedToken is JwtSecurityToken securityToken)
                && securityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256,
                StringComparison.InvariantCultureIgnoreCase);
        }
    }
}
