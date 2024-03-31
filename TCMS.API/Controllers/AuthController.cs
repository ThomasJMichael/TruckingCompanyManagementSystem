using Microsoft.AspNetCore.Mvc;
using TCMS.Common.DTOs.User; 
using TCMS.Services.Interfaces;

namespace TCMS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto loginDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _authService.LoginAsync(loginDto);

            if (!result.IsSuccessful)
            {
                return BadRequest(result.Messages);
            }

            return Ok();
        }

        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            await _authService.LogoutAsync();
            return Ok();
        }

        [HttpPost("change-password")]
        public async Task<IActionResult> ChangePassword(ChangePasswordDto changePasswordDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _authService.ChangePasswordAsync(changePasswordDto);

            if (!result.IsSuccessful)
            {
                return BadRequest(result.Messages);
            }

            return Ok();
        }
    }
}
