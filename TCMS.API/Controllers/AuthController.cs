using Microsoft.AspNetCore.Mvc;
using TCMS.Common.DTOs.User;
using TCMS.Common.Operations;
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
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(OperationResult))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<OperationResult<UserAccountDto>>> Login(LoginDto loginDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _authService.LoginAsync(loginDto);

            if (!result.IsSuccessful)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }

        [HttpPost("logout")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(OperationResult))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<OperationResult>> Logout()
        {
            var result = await _authService.LogoutAsync();
            return result.IsSuccessful ? Ok(result) : BadRequest(result);
        }

        [HttpPost("change-password")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(OperationResult))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<OperationResult>> ChangePassword(ChangePasswordDto changePasswordDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _authService.ChangePasswordAsync(changePasswordDto);

            if (!result.IsSuccessful)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }
    }
}
