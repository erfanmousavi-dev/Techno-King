using App.Domain.Core.Techno_King.DTOs.Users;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Domain.Core.Techno_King.App.Domain.Core
{
    public interface IUserAppService
    {
        // امضای جدید با Tuple برای خروجی توکن و نتیجه
        Task<(IdentityResult Result, AuthResponseDTO? TokenData)> Register(UserForRegisterDTO model, CancellationToken cancellationToken);

        // امضای جدید برای لاگین
        Task<AuthResponseDTO?> Login(string email, string password);

        // امضای جدید برای رفرش توکن
        Task<AuthResponseDTO?> RefreshTokenAsync(RefreshTokenRequestDTO model);

        Task<List<GetUserBaseForViewPage>> GetAllUsersAsync(CancellationToken cancellationToken);
        Task<UserDTO?> GetCurrentUserAsync();
        Task<UserBaseDTO> GetByIdAsync(int id, CancellationToken cancellationToken);
        Task LogoutAsync();
        Task<IdentityResult> UpdateUserInfo(UpdateUserInfoDTO userDto, int userId, CancellationToken cancellationToken);
        Task<IdentityResult> DeleteUser(int UserId, CancellationToken cancellationToken);
    }
}
