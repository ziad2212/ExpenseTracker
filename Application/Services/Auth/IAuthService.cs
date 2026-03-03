using ExpenseTracker.Application.DTOs.Auth;
using ExpenseTracker.Domain.Users;

namespace ExpenseTracker.Application.Services.Auth
{
    public interface IAuthService
    {
        Task<AuthResponse> Register(RegisterRequest request);
        Task<AuthResponse> Login(LoginRequest request);
    }
}