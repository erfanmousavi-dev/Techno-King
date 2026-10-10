using App.Domain.Core.Techno_King.Entities.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace App.Domain.Core.Techno_King.Service
{
    public interface IJwtTokenGenerator
    {
        string GenerateAccessToken(UserBase user, IList<string> roles, IEnumerable<Claim> userClaims);
        string GenerateRefreshToken();
        ClaimsPrincipal? GetPrincipalFromExpiredToken(string token);
    }
}
