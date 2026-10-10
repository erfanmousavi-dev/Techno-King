using App.Domain.Core.Techno_King.App.Domain.Core;
using App.Domain.Core.Techno_King.DTOs.Users;
using App.Domain.Core.Techno_King.Entities.Users;
using App.Domain.Core.Techno_King.Enum;
using App.Domain.Core.Techno_King.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Techno_KingService.Techno_King.Users;

namespace Techno_KingAppService.Techno_King.Users
{
    public class UserAppService : IUserAppService
    {
        #region Dependency Injection
        private readonly UserManager<UserBase> _userManager;
        private readonly IPasswordHasher<UserBase> _passwordHasher;
        private readonly ILogger<UserAppService> _logger;
        private readonly IMemoryCache _memoryCache;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IGeneralService _generalService;
        private readonly IUserService _userService;
        private readonly IJwtTokenGenerator _jwtTokenGenerator;
        private readonly IConfiguration _configuration;

        public UserAppService(
            UserManager<UserBase> userManager,
            IPasswordHasher<UserBase> passwordHasher,
            ILogger<UserAppService> logger,
            IMemoryCache memoryCache,
            IHttpContextAccessor httpContextAccessor,
            IGeneralService generalService,
            IUserService userService,
            IJwtTokenGenerator jwtTokenGenerator,
            IConfiguration configuration)
        {
            _userManager = userManager;
            _passwordHasher = passwordHasher;
            _logger = logger;
            _memoryCache = memoryCache;
            _httpContextAccessor = httpContextAccessor;
            _generalService = generalService;
            _userService = userService;
            _jwtTokenGenerator = jwtTokenGenerator;
            _configuration = configuration;
        }
        #endregion

        #region Helper Methods
        private async Task<AuthResponseDTO> GenerateAndSaveTokensAsync(UserBase user)
        {
            var roles = await _userManager.GetRolesAsync(user);
            var claims = await _userManager.GetClaimsAsync(user);

            var accessToken = _jwtTokenGenerator.GenerateAccessToken(user, roles, claims);
            var refreshToken = _jwtTokenGenerator.GenerateRefreshToken();

            var days = double.Parse(_configuration["JwtSettings:RefreshTokenExpirationDays"]!);

            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(days);

            await _userManager.UpdateAsync(user);

            return new AuthResponseDTO
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken,
                AccessTokenExpiration = DateTime.UtcNow.AddMinutes(double.Parse(_configuration["JwtSettings:AccessTokenExpirationMinutes"]!))
            };
        }

        private string UpdateIfChanged(string currentValue, string newValue)
        {
            if (!string.IsNullOrWhiteSpace(newValue) && currentValue != newValue)
            {
                return newValue;
            }
            return currentValue;
        }
        #endregion

        #region Create
        public async Task<(IdentityResult Result, AuthResponseDTO? TokenData)> Register(UserForRegisterDTO model, CancellationToken cancellationToken)
        {
            var user = new UserBase
            {
                UserName = model.UserName,
                Email = model.Email,
                Role = RoleEnum.Customer,
                Balance = 1000000,
                RegisteredAt = DateTime.Now,
                Customer = new Customer()
            };

            var result = await _userManager.CreateAsync(user, model.Password);

            if (!result.Succeeded)
            {
                _logger.LogWarning("User with username {Username} failed to register at {Time}",
                    model.UserName, DateTime.UtcNow.ToLongTimeString());
                return (result, null);
            }

            await _userManager.AddToRoleAsync(user, "Customer");
            await _userManager.AddClaimAsync(user, new Claim("CustomerId", user.Customer.Id.ToString()));

            AuthResponseDTO? tokenData = null;
            if (!model.CreatedByAdmin)
            {
                tokenData = await GenerateAndSaveTokensAsync(user);
            }

            _logger.LogInformation("User with username {Username} registered successfully at {Time}",
                user.UserName, DateTime.UtcNow.ToLongTimeString());

            return (result, tokenData);
        }
        #endregion

        #region Read
        #region Login
        public async Task<AuthResponseDTO?> Login(string email, string password)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null || !await _userManager.CheckPasswordAsync(user, password))
            {
                _logger.LogWarning("Failed login attempt with email {Email}", email);
                return null;
            }

            _logger.LogInformation("User with email {Email} logged in successfully", email);
            return await GenerateAndSaveTokensAsync(user);
        }

        public async Task<AuthResponseDTO?> RefreshTokenAsync(RefreshTokenRequestDTO model)
        {
            var principal = _jwtTokenGenerator.GetPrincipalFromExpiredToken(model.AccessToken);
            if (principal is null) return null;

            var userIdClaim = principal.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdClaim)) return null;

            var user = await _userManager.FindByIdAsync(userIdClaim);

            if (user is null || user.RefreshToken != model.RefreshToken || user.RefreshTokenExpiryTime <= DateTime.UtcNow)
            {
                _logger.LogWarning("Invalid refresh token attempt for user ID {UserId}", userIdClaim);
                return null;
            }

            return await GenerateAndSaveTokensAsync(user);
        }
        #endregion

        public async Task<List<GetUserBaseForViewPage>> GetAllUsersAsync(CancellationToken cancellationToken)
        {
            List<GetUserBaseForViewPage> WantedUsers;
            if (_memoryCache.Get("AllUsers") is not null)
            {
                WantedUsers = _memoryCache.Get<List<GetUserBaseForViewPage>>("AllUsers");
            }
            else
            {
                WantedUsers = await _userService.GetAllAsync(cancellationToken);
                _memoryCache.Set("AllUsers", WantedUsers,
                    new MemoryCacheEntryOptions
                    {
                        SlidingExpiration = TimeSpan.FromSeconds(10)
                    }
                );
            }
            return WantedUsers;
        }

        public async Task<UserDTO?> GetCurrentUserAsync()
        {
            var user = _httpContextAccessor.HttpContext?.User;
            if (user == null || !user.Identity.IsAuthenticated)
            {
                return null;
            }

            string cacheKey = $"User_{user.Identity.Name}";
            if (!_memoryCache.TryGetValue(cacheKey, out UserDTO userDTO))
            {
                var applicationUser = await _userManager.GetUserAsync(user);
                if (applicationUser == null)
                {
                    return null;
                }

                userDTO = new UserDTO
                {
                    Id = applicationUser.Id,
                    FullName = applicationUser.FirstName + " " + applicationUser.LastName ?? applicationUser.Email ?? "Anonymous User",
                    ProfileImageUrl = applicationUser.ImagePath ?? "~/images/Profiles/dummy-avatar.jpg"
                };

                var cacheOptions = new MemoryCacheEntryOptions()
                    .SetSlidingExpiration(TimeSpan.FromMinutes(10));
                _memoryCache.Set(cacheKey, userDTO, cacheOptions);
            }

            return userDTO;
        }

        public async Task<UserBaseDTO> GetByIdAsync(int id, CancellationToken cancellationToken)
        {
            return await _userService.GetByIdAsync(id, cancellationToken);
        }
        #endregion

        #region Update
        public async Task LogoutAsync()
        {
            // Stateless JWT logout is handled on client side by discarding tokens.
            await Task.CompletedTask;
        }

        public async Task<IdentityResult> UpdateUserInfo(UpdateUserInfoDTO userDto, int userId, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user == null)
            {
                _logger.LogWarning("User with ID {UserId} was not found.", userId);
                return IdentityResult.Failed(new IdentityError { Description = "User not found." });
            }

            user.UserName = UpdateIfChanged(user.UserName, userDto.UserName);
            user.FirstName = UpdateIfChanged(user.FirstName, userDto.FirstName);
            user.LastName = UpdateIfChanged(user.LastName, userDto.LastName);
            user.Email = UpdateIfChanged(user.Email, userDto.Email);
            user.Mobile = UpdateIfChanged(user.Mobile, userDto.Mobile);

            var result = await _userManager.UpdateAsync(user);

            if (result.Succeeded)
            {
                _logger.LogInformation("User with ID {UserId} updated successfully at {Time}.", userId, DateTime.UtcNow.ToLongTimeString());
            }
            else
            {
                _logger.LogWarning("Failed to update user with ID {UserId} at {Time}.", userId, DateTime.UtcNow.ToLongTimeString());
            }

            return result;
        }
        #endregion

        #region Delete
        public async Task<IdentityResult> DeleteUser(int UserId, CancellationToken cancellationToken)
        {
            var StringId = Convert.ToString(UserId);
            var user = await _userManager.FindByIdAsync(StringId);
            if (user == null)
            {
                return IdentityResult.Failed(new IdentityError { Description = "User not found." });
            }

            user.IsDeleted = true;
            var result = await _userManager.UpdateAsync(user);
            return result;
        }
        #endregion
    }
}