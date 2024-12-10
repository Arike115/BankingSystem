using BankingSystem.Domain.Entities;
using BankingSystem.Domain.Model;
using BankingSystem.Interface;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using ILogger = Serilog.ILogger;

namespace BankingSystem.Application.Commands.UserRegistration
{
    public class LoginCommandHandler : IRequestHandler<LoginCommand, BaseResponse<JsonWebToken>>
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IMediator _mediator;
        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly IJwtHandler _jwtHandler;

        public LoginCommandHandler(UserManager<ApplicationUser> userManager,
        RoleManager<ApplicationRole> roleManager,
        SignInManager<ApplicationUser> signInManager,
        IMediator mediator,
        IConfiguration configuration,
        IWebHostEnvironment hostingEnvironment, IJwtHandler jwtHandler)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _mediator = mediator;
            _configuration = configuration;
            _hostingEnvironment = hostingEnvironment;
            _jwtHandler = jwtHandler;
        }
        /// <summary>
        /// this method is use to log the user in, do the neccessary checks for the user and also generate a token
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<BaseResponse<JsonWebToken>> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByNameAsync(request.Email)
             ?? await _userManager.FindByEmailAsync(request.Email);

            if (user == null)
            {
                // User not found
                return new BaseResponse<JsonWebToken>(false, "Invalid username or password.");
            }



            // Checking if the password is correct
            var isPasswordValid = await _userManager.CheckPasswordAsync(user, request.Password);
            if (!isPasswordValid)
            {
                return new BaseResponse<JsonWebToken>(false, "Invalid username or password");
            }


            // If login is successful, generate JWT token
            var authClaims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.UserName),
                    new Claim(System.IdentityModel.Tokens.Jwt.JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                };

            var userRoles = await _userManager.GetRolesAsync(user);
            foreach (var role in userRoles)
            {
                authClaims.Add(new Claim(ClaimTypes.Role, role));
            }

            var tokenResult = await this.GenerateAuthResponseAsync(user);
            return new BaseResponse<JsonWebToken>(true,"User Successfully saved",tokenResult);

        }

        /// <summary>
        /// Generate the token for the user
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public async Task<Domain.Model.JsonWebToken> GenerateAuthResponseAsync(ApplicationUser user)
        {
            List<Claim> claims = new List<Claim>();
            bool islogin = true;

            var roleNames = await _userManager.GetRolesAsync(user);
            var roles = _roleManager.Roles.Where(x => roleNames.Contains(x.Name)).ToList();

            foreach (ApplicationRole role in roles)
            {
                var roleClaims = await _roleManager.GetClaimsAsync(role);
                claims.AddRange(roleClaims);
            }
            
            // await _userManager.ResetAccessFailedCountAsync(user);
            //generate token
            var tokenResult = _jwtHandler.Create(
                user.Id, user.Email, $"{user.FirstName} {user.LastName}", user.BusinessName,
               user.PhoneNumber,
                claims, 
                islogin
            );

            return tokenResult;
        }

       
    }
}
