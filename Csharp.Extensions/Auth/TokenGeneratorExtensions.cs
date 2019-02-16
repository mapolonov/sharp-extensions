using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace Csharp.Extensions.Auth
{
    public static class TokenGeneratorExtensions
    {
        public static string GenerateToken(this BearerOptions options, UserAuthProfile userAuthProfile)
        {
            if (options == null) throw new ArgumentNullException(nameof(options));
            if (userAuthProfile == null) throw new ArgumentNullException(nameof(userAuthProfile));

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, userAuthProfile.Name),
                new Claim(ClaimTypes.Email, userAuthProfile.Email),
            };

            var roles = userAuthProfile.Roles.Select((role) => new Claim(ClaimTypes.Role, role));
            claims.AddRange(roles);

            var token = new JwtSecurityToken
            (
                options.ValidIssuer,
                options.Audience,
                expires: DateTime.UtcNow.AddDays(options.Expires),
                claims: claims,
                notBefore: DateTime.UtcNow,
                signingCredentials: new SigningCredentials(
                    new SymmetricSecurityKey(Encoding.Default.GetBytes(options.SecretKey)),
                    SecurityAlgorithms.HmacSha256Signature)

            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
