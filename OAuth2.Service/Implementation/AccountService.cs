using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using OAuth2.Domain.Auth;
using OAuth2.Domain.Common;
using OAuth2.Domain.Models;
using OAuth2.Domain.Request;
using OAuth2.Domain.Responses;
using OAuth2.Infrastructure.Transforms;
using OAuth2.Infrastructure.Validators;
using OAuth2.Persistence;
using OAuth2.Service.Contract;
using OAuth2.Service.Transforms;
using OAuth2.Service.Validators;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace OAuth2.Service.Implementation
{
    public class AccountService : IAccountService
    {
        private readonly IOAuth2DbContext _context;
        private readonly IPasswordHasher<User> _passwordHasher;
        private readonly IConfiguration _configuration;

        public AccountService(IOAuth2DbContext pContext, IPasswordHasher<User> pPasswordHasher,
            IConfiguration pConfiguration) 
        {
            _context = pContext;
            _passwordHasher = pPasswordHasher;
            _configuration = pConfiguration;
        }

        public async Task<Result<RegistrationResponses>> RegisterAsync(RegisterRequest pRequest)
        {
            var validator = new RegisterRequestValidator(_context);
            var validationResult = await validator.ValidateAsync(pRequest);

            if (validationResult.IsValid == false)
            {
                var errorMessages = validationResult.Errors.Select(x => x.ErrorMessage).ToList();
                return Result<RegistrationResponses>.Failure(errorMessages, StatusCodes.Status400BadRequest);
            }

            try
            {
                var user = new User
                {
                    FirstName = pRequest.FirstName,
                    LastName = pRequest.LastName,
                    Email = pRequest.Email,
                    PhoneNumber = pRequest.PhoneNumber,
                };
                if(pRequest.IsEmail == true)
                {
                    user.Email = pRequest.Email;
                }
                else
                {
                    user.PhoneNumber = pRequest.PhoneNumber;
                }

                var hashedPassword = _passwordHasher.HashPassword(user, pRequest.Password);
                user.Password = hashedPassword;

                var result = await _context.Users.AddAsync(user);
                await _context.SaveChangesAsync();

                return Result<RegistrationResponses>
                        .Success(new RegistrationResponses() { Id = result.Entity.Id },
                        StatusCodes.Status201Created);
            }
            catch (Exception ex)
            {
                return Result<RegistrationResponses>.Failure(ex.Message, StatusCodes.Status500InternalServerError);
            }
        }

        public async Task<Result<LoginResponses>> LoginAsync(LoginRequest pRequest)
        {
            try
            {
                User user = null;
                if (pRequest.IsEmail == true)
                {
                    user = await _context.Users.FirstOrDefaultAsync(x => x.Email == pRequest.Email);
                }
                else
                {
                    user = await _context.Users.FirstOrDefaultAsync(x => x.PhoneNumber == pRequest.PhoneNumber);
                }

                if (user == null)
                {
                    return Result<LoginResponses>
                        .Failure(IdentityTransform.UserNotExists(pRequest.IsEmail == true ? pRequest.Email : pRequest.PhoneNumber),
                        StatusCodes.Status400BadRequest);
                }
                var result = _passwordHasher.VerifyHashedPassword(user, user.Password, pRequest.Password);

                if (result != PasswordVerificationResult.Success)
                {
                    return Result<LoginResponses>
                        .Failure(IdentityTransform.InvalidCredentials(pRequest.IsEmail == true ? pRequest.Email : pRequest.PhoneNumber),
                        StatusCodes.Status400BadRequest);
                }

                JwtSecurityToken jwtSecurityToken = await GenerateToken(user);

                LoginResponses auth = new LoginResponses
                {
                    Id = user.Id,
                    Token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken),
                };

                return Result<LoginResponses>.Success(auth, StatusCodes.Status200OK);

            }
            catch (Exception ex)
            {
                return Result<LoginResponses>.Failure(ex.Message, StatusCodes.Status500InternalServerError);
            }
        }

        public async Task<Result<LoginResponses>> GoogleLoginAsync(GoogleLoginRequest pRequest)
        {
            try
            {
                // Validate external access token with Google
                var googleUser = await ValidateGoogleTokenAsync(pRequest.ExternalAccessToken);

                if (googleUser == null)
                {
                    return Result<LoginResponses>.Failure("Không thể xác thực mã thông báo Google!", StatusCodes.Status400BadRequest);
                }

                // Check if a user with the Google email exists in the database
                var user = await _context.Users.FirstOrDefaultAsync(x => x.Email == googleUser.Email);

                if (user == null)
                {
                    // If the user doesn't exist, you may choose to create a new user account here.
                    // For simplicity, we'll return an error for now.
                    return Result<LoginResponses>.Failure("User not found!", StatusCodes.Status400BadRequest);
                }

                JwtSecurityToken jwtSecurityToken = await GenerateToken(user);

                LoginResponses auth = new LoginResponses
                {
                    Id = user.Id,
                    Token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken),
                };

                return Result<LoginResponses>.Success(auth, StatusCodes.Status200OK);
            }
            catch (Exception ex)
            {
                return Result<LoginResponses>.Failure(ex.Message, StatusCodes.Status500InternalServerError);
            }
        }

        private async Task<GoogleUserModel> ValidateGoogleTokenAsync(string externalAccessToken)
        {
            using (var httpClient = new HttpClient())
            {
                var response = await httpClient.GetAsync($"https://www.googleapis.com/oauth2/v3/tokeninfo?access_token={externalAccessToken}");
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<GoogleUserModel>(content);
                }
                return null;
            }
        }

        private async Task<JwtSecurityToken> GenerateToken(User pUser)
        {
            var roles = await _context.Roles
                    .Where(x => x.UserRoles.Any(x => x.UserId == pUser.Id))
                    .ToListAsync();
            var permissions = await _context.Permissions
                    .Where(x => x.UserPermissions.Any(x => x.UserId == pUser.Id) ||
                                x.RolePermissions.Any(x => x.Role.UserRoles.Any(x => x.UserId == pUser.Id)))
                    .ToListAsync();

            var roleClaims = roles.Select(role => new Claim(ClaimTypes.Role, role.Name));
            var permissionClaims = permissions.Select(permission => new Claim(ClaimTransform.Permission, permission.Name));

            var claims = new[]
            {
                new Claim(ClaimTransform.Uid, pUser.Id.ToString()),
            }
            .Union(permissionClaims)
            .Union(roleClaims);

            var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtSettings:Key"]));
            var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);

            var jwtSecurityToken = new JwtSecurityToken(
                issuer: _configuration["JwtSettings:Issuer"],
                audience: _configuration["JwtSettings:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(int.Parse(_configuration["JwtSettings:DurationInMinutes"])),
                signingCredentials: signingCredentials);
            return jwtSecurityToken;
        }
    }
}
