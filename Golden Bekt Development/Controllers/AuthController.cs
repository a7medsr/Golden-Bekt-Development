using Golden_Bekt_Development.DTOs;
using Golden_Bekt_Development.Models;
using Golden_Bekt_Development.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
namespace Golden_Bekt_Development.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAdminAuthService _authService;

        public AuthController(IAdminAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto request)
        {
            try
            {
                var admin = await _authService.RegisterAsync(request.UserName, request.Password, request.Name);
                return Ok(new
                {
                    admin.Id,
                    admin.Name,
                    admin.UserName
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto request)
        {
            var token = await _authService.LoginAsync(request.UserName, request.Password);

            if (token == null)
                return Unauthorized(new { message = "Invalid username or password" });

            return Ok(new { token });
        }
    }
}