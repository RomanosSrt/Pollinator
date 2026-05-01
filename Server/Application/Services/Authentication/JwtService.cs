using API.Application.DTOs.UserInsertion;
using API.Domain.Entities.UserManagement;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace API.Application.Services.Authentication
{
    public class JwtService : IJwtService
    {
        private readonly ILogger<JwtService> _logger;
        private readonly IConfiguration _config;
        public JwtService(ILogger<JwtService> logger, IConfiguration config)
        {
            _logger = logger;
            _config = config;
        }

        public AuthResultDto GenerateToken(ApplicationUser user)
        {
            _logger.LogInformation("IN JwTService - Generating JWT token for user with ID: {UserId} and email: {Email}", user.Id, user.Email);
            var jwtKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]
                ?? throw new InvalidOperationException("JWT key is not configured.")));

            var creds = new SigningCredentials(jwtKey, SecurityAlgorithms.HmacSha256);    

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id),
                new Claim(JwtRegisteredClaimNames.Email, user.Email!),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddHours(24),
                signingCredentials: creds
            );
            return new AuthResultDto
            {
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                ExpiresAt = token.ValidTo,
                UserId = user.Id,
                Email = user.Email!,
                UserType = user.userType.ToString()
            };
        }
    }
}
