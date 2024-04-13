using Microsoft.AspNetCore.Mvc;
using TCMS.Common.DTOs.Employee;
using TCMS.Common.Operations;
using TCMS.Services.Interfaces;

namespace TCMS.API.Controllers
{
    [Route("api/employee")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly IEmployeeService _employeeService;

        public EmployeeController(IEmployeeService employeeService)
        {
            _employeeService = employeeService;
        }

        // Get all employees
        [HttpGet("all")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(OperationResult<IEnumerable<EmployeeDto>>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<OperationResult<IEnumerable<EmployeeDto>>>> GetAllEmployees()
        {
            var result = await _employeeService.GetEmployeesAsync();
            return result.IsSuccessful ? Ok(result) : BadRequest(result);
        }

        // Get an employee by ID
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(OperationResult<EmployeeDto>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<OperationResult<EmployeeDto>>> GetEmployeeById(string id)
        {
            var result = await _employeeService.GetEmployeeByIdAsync(id);
            if (!result.IsSuccessful)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        // Update an employee
        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(OperationResult<EmployeeDto>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<OperationResult<EmployeeDto>>> UpdateEmployee([FromBody] EmployeeDto employeeDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _employeeService.UpdateEmployeeAsync(employeeDto);
            return result.IsSuccessful ? Ok(result) : BadRequest(result);
        }
    }
}

