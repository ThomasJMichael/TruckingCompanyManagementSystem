using Microsoft.AspNetCore.Mvc;
using TCMS.Common.DTOs.Financial;
using TCMS.Common.Operations;
using TCMS.Services.Interfaces;

namespace TCMS.API.Controllers
{
    [Route("api/payroll")]
    [ApiController]
    public class PayrollController : ControllerBase
    {
        private readonly IPayrollService _payrollService;

        public PayrollController(IPayrollService payrollService)
        {
            _payrollService = payrollService;
        }

        [HttpGet("all")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(OperationResult))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetAllPayrolls()
        {
            var result = await _payrollService.GetAllPayrollsAsync();
            return result.IsSuccessful ? Ok(result) : BadRequest(result);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(OperationResult))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetPayrollById(int id)
        {
            var result = await _payrollService.GetPayrollByIdAsync(id);
            return result.IsSuccessful ? Ok(result) : BadRequest(result);
        }

        // Generate payroll for period
        [HttpPost("generate")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(OperationResult))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GeneratePayrollForPeriod(DateTime payPeriodStart, DateTime payPeriodEnd)
        {
            var result = await _payrollService.GeneratePayrollForPeriodAsync(payPeriodStart, payPeriodEnd);
            return result.IsSuccessful ? Ok(result) : BadRequest(result);
        }

        [HttpPut("update")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(OperationResult))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdatePayroll([FromBody] PayrollDto payroll)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _payrollService.UpdatePayrollAsync(payroll);
            return result.IsSuccessful ? Ok(result) : BadRequest(result);
        }

        [HttpDelete("delete/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(OperationResult))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> DeletePayroll(int id)
        {
            var result = await _payrollService.DeletePayrollAsync(id);
            return result.IsSuccessful ? Ok(result) : BadRequest(result);
        }

    }
}
