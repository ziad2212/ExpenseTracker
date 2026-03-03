using ExpenseTracker.Application.DTOs.Auth;
using ExpenseTracker.Application.Services.Auth;
using Microsoft.AspNetCore.Mvc;

namespace ExpenseTracker.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request)
        {
            try
            {
                var response = await _authService.Register(request);
                return Ok(response);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(ex.Message); // 401
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message); // 400
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message); // unexpected errors
            }
        }
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            try
            {
                var response = await _authService.Login(request);
                return Ok(response);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(ex.Message); // 401
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message); // 400
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message); // unexpected errors
            }
        }
    }
}
