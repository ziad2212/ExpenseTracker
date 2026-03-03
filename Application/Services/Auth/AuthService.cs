using System;
using System.Text;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using ExpenseTracker.Domain.Expenses;
using Microsoft.EntityFrameworkCore;
using ExpenseTracker.Infrastructure.Data;
using ExpenseTracker.Domain.Users;
using Microsoft.Extensions.Configuration;
using ExpenseTracker.Application.DTOs.Auth;

namespace ExpenseTracker.Application.Services.Auth
{
    public class AuthService : IAuthService
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _configuration;

        public AuthService(AppDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        public async Task<AuthResponse> Login(LoginRequest request)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }
            if (string.IsNullOrEmpty(request.Email) || string.IsNullOrEmpty(request.Password))
            {
                throw new ArgumentException("Email and password must be provided.");
            }
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == request.Email);
            if (user == null || !BCrypt.Net.BCrypt.Verify(request.Password, user.Password))
            {
                throw new UnauthorizedAccessException("Invalid email or password.");
            }


            var token = GenerateToken(user);
            return new AuthResponse { Token = token };
        }

        private string GenerateToken(User user)
        {
            if (user == null) throw new ArgumentNullException(nameof(user));

            var key = _configuration["JwtSettings:SecretKey"];
            if (string.IsNullOrEmpty(key))
            {
                throw new InvalidOperationException("JWT signing key is not configured. Set configuration key 'Jwt:Key'.");
            }

            var issuer = _configuration["JwtSettings:Issuer"] ?? "ExpenseTracker";
            var audience = _configuration["JwtSettings:Audience"] ?? "ExpenseTrackerUsers";
            var expiresInMinutes = 60;
            if (int.TryParse(_configuration["JwtSettings:ExpiresInMinutes"], out var parsed))
            {
                expiresInMinutes = parsed;
            }

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email ?? string.Empty),
                new Claim("name", user.Name ?? string.Empty),
                // Add more claims as needed, e.g., roles, permissions, etc.
                // jti (JWT ID) claim to ensure token uniqueness
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var tokenDescriptor = new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                claims: claims,
                notBefore: DateTime.UtcNow,
                expires: DateTime.UtcNow.AddMinutes(expiresInMinutes),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(tokenDescriptor);
        }

        public async Task<AuthResponse> Register(RegisterRequest request)
        {
            if(request == null) throw new ArgumentNullException(nameof(request));
            if (string.IsNullOrEmpty(request.Name) || string.IsNullOrEmpty(request.Email) || string.IsNullOrEmpty(request.Password))
            {
                throw new ArgumentException("Name, email, and password must be provided.");
            }
            if (await _context.Users.AnyAsync(u => u.Email == request.Email))
            {
                throw new InvalidOperationException("A user with this email already exists.");
            }
            var user = new User
            {
                Name = request.Name,
                Email = request.Email,
                Password = BCrypt.Net.BCrypt.HashPassword(request.Password)
            };
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            var token = GenerateToken(user);
            return new AuthResponse { Token = token };
        }
    }
}
