using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using TCMS.Common.DTOs.Employee;
using TCMS.Common.Operations;
using TCMS.Data.Data;
using TCMS.Services.Interfaces;

namespace TCMS.Services.Implementations
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IMapper _mapper;
        private readonly TcmsContext _context;

        public EmployeeService(IMapper mapper, TcmsContext context)
        {
            _mapper = mapper;
            _context = context;
        }

        public async Task<OperationResult<IEnumerable<EmployeeDto>>> GetEmployeesAsync()
        {
            try
            {
                var employees = _context.Employees.ToList();
                var employeeDtos = _mapper.Map<IEnumerable<EmployeeDto>>(employees);
                return OperationResult<IEnumerable<EmployeeDto>>.Success(employeeDtos);
            }
            catch (Exception e)
            {
                return OperationResult<IEnumerable<EmployeeDto>>.Failure([e.Message]);
            }
        }

        public async Task<OperationResult<EmployeeDto>> GetEmployeeByIdAsync(string id)
        {
            try
            {
                var employee = await _context.Employees.FirstOrDefaultAsync(e => e.EmployeeId == id);
                if (employee == null)
                {
                    return OperationResult<EmployeeDto>.Failure(new List<string> { "Employee not found." });
                }

                var employeeDto = _mapper.Map<EmployeeDto>(employee);
                return OperationResult<EmployeeDto>.Success(employeeDto);
            }
            catch (Exception e)
            {
                return OperationResult<EmployeeDto>.Failure(new List<string> { e.Message });
            }
        }


        public async Task<OperationResult<EmployeeDto>> UpdateEmployeeAsync(EmployeeDto employee)
        {
            try
            {
                var employeeToUpdate = _context.Employees.FirstOrDefault(e => e.EmployeeId == employee.EmployeeId);
                if (employeeToUpdate == null)
                {
                    return OperationResult<EmployeeDto>.Failure(["Employee not found."]);
                }

                _mapper.Map(employee, employeeToUpdate);

                _context.Employees.Update(employeeToUpdate);
                await _context.SaveChangesAsync();

                return OperationResult<EmployeeDto>.Success(employee);
            }
            catch (Exception e)
            {
                return OperationResult<EmployeeDto>.Failure([e.Message]);
            }
        }
    }
}
