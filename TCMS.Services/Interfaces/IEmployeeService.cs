using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TCMS.Common.DTOs.Employee;
using TCMS.Common.Operations;

namespace TCMS.Services.Interfaces
{
    public interface IEmployeeService
    {
        public Task<OperationResult<IEnumerable<EmployeeDto>>> GetEmployeesAsync();
        public Task<OperationResult<EmployeeDto>> GetEmployeeByIdAsync(string id);
        public Task<OperationResult<EmployeeDto>> UpdateEmployeeAsync(EmployeeDto employee);
    }
}
