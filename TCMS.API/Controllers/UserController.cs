using System.Runtime.CompilerServices;
using Microsoft.AspNetCore.Mvc;
using TCMS.Common.DTOs.Employee;
using TCMS.Common.DTOs.User;
using TCMS.Common.Operations;
using TCMS.Services.Interfaces;

namespace TCMS.API.Controllers
{
    [Route("api/user")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        // Create a new user
        [HttpPost("create")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(OperationResult))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<OperationResult>> CreateUser([FromBody] NewAccountDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _userService.CreateUserAccountAsync(dto);
            return result.IsSuccessful ? Ok(result) : BadRequest(result);
        }

        // Update the user
        [HttpPut("update")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(OperationResult))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<OperationResult>> UpdateUsername([FromBody] ChangeUsernameDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _userService.UpdateUsernameAsync(dto);
            return result.IsSuccessful ? Ok(result) : BadRequest(result);
        }

        // Delete the user
        [HttpDelete("delete/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(OperationResult))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<OperationResult>> DeleteUser(string id)
        {
            var result = await _userService.DeleteUserAccountAsync(id);
            return result.IsSuccessful ? Ok(result) : BadRequest(result);
        }

        // Update pay rate
        [HttpPut("update-pay-rate")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(OperationResult))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<OperationResult>> UpdatePayRate([FromBody] UpdatePayRateDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _userService.UpdatePayRate(dto);
            return result.IsSuccessful ? Ok(result) : BadRequest(result);
        }

        // Add role to user
        [HttpPost("add-role")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(OperationResult))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<OperationResult>> AddRole([FromBody] UserRoleDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _userService.AddRoleToUserAsync(dto);
            return result.IsSuccessful ? Ok(result) : BadRequest(result);
        }

        // Remove role from user
        [HttpDelete("remove-role")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(OperationResult))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<OperationResult>> RemoveRole([FromBody] UserRoleDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _userService.RemoveRoleFromUserAsync(dto);
            return result.IsSuccessful ? Ok(result) : BadRequest(result);
        }

        // Get Roles for User
        [HttpGet("roles/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(OperationResult<IEnumerable<string>>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<OperationResult<IEnumerable<string>>>> GetRolesForUser(string id)
        {
            var result = await _userService.GetRolesAsync(id);
            return result.IsSuccessful ? Ok(result) : BadRequest(result);
        }

        // Get user account by id
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(OperationResult<UserAccountDto>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<OperationResult<UserAccountDto>>> GetUserById(string id)
        {
            var result = await _userService.GetUserAccountByIdAsync(id);
            return result.IsSuccessful ? Ok(result) : BadRequest(result);
        }

        // Get all user accounts
        [HttpGet("all")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(OperationResult<IEnumerable<UserAccountDto>>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<OperationResult<IEnumerable<UserAccountDto>>>> GetAllUsers()
        {
            var result = await _userService.GetAllUserAccountsAsync();
            return result.IsSuccessful ? Ok(result) : BadRequest(result);
        }
    }

}
