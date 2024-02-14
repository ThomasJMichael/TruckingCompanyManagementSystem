
using TCMS.Data.Models;

namespace TCMS.Services.Interfaces;

public interface IEmployeeService
{
    Task<IEnumerable<Employee>> GetAll();
    Task<Employee> GetByIdAsync (int employeeId);
    Task<Employee> CreateAsync(Employee employee);
    Task<bool> UpdateAsync(Employee employee);
    Task<bool> DeleteAsync(int employeeId);

    Task<IEnumerable<TimeSheet>> GetTimeSheetsByEmployeeIdAsync (int employeeId);
    Task<TimeSheet> AddTimeSheetAsync (int employeeId, TimeSheet timeSheet);
    Task<bool> UpdateTimeSheetAsync (TimeSheet timeSheet);
    Task<bool> DeleteTimeSheetAsync (int timeSheetId);

    
}