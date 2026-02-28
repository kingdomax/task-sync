using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

using TaskSync.Infrastructure.Settings;
using TaskSync.Repositories.Entities;
using TaskSync.Services.Interfaces;

namespace TaskSync.Services
{
    public class JwtService : IJwtService
    {
        private readonly JwtSettings _jwtSettings;

        public JwtService(IOptions<JwtSettings> options)
        {
            _jwtSettings = options.Value;
        }

        public string GenerateJwtToken(UserEntity user)
        {
            var claims = new[] // store only necessary and un-changed information so the token is small and not out of sync
            {
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.SecretKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256); // MAC not hashing

            var token = new JwtSecurityToken(
                issuer: _jwtSettings.Issuer,
                audience: _jwtSettings.Audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(_jwtSettings.ExpirationMinutes),
                signingCredentials: creds
            );

            // Ex. eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiIxMjMiLCJuYW1lIjoiam9obiIsImV4cCI6MTcxMDAwMDAwMH0.Dp0Y5W6wzH2pJ9Z8R1R0o9nYz0o5xFQ4vR4Gk2U6xKQ
            // The token consists of three parts: header, payload, and signature, separated by dots. It is encoding based.
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
