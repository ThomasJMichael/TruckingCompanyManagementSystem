
using TCMS.Common.DTOs.Employee;
using TCMS.Common.DTOs.Misc;
using TCMS.Data.Models;

namespace TCMS.Services.Interfaces;

public interface IEmployeeDtoService
{
    Task<IEnumerable<EmployeeDto>> GetAll();
    Task<EmployeeDto> GetByIdAsync (int employeeId);
    Task<EmployeeDto> CreateAsync(EmployeeDto employee);
    Task<bool> UpdateAsync(EmployeeDto employee);
    Task<bool> DeleteAsync(int employeeId);

    Task<IEnumerable<TimeSheetDto>> GetTimeSheetsByEmployeeDtoIdAsync (int employeeId);
    Task<TimeSheetDto> AddTimeSheetAsync (int employeeId, TimeSheetDto timeSheet);
    Task<bool> UpdateTimeSheetDtoAsync (TimeSheetDto timeSheet);
    Task<bool> DeleteTimeSheetDtoAsync (int timeSheetId);

    
}