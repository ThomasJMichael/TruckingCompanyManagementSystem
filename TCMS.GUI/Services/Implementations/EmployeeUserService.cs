using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using TCMS.Common.DTOs.Employee;
using TCMS.Common.DTOs.User;
using TCMS.Common.Operations;
using TCMS.GUI.Models;
using TCMS.GUI.Services.Interfaces;

namespace TCMS.GUI.Services.Implementations
{
    public class EmployeeUserService : IEmployeeUserService
    {
        private readonly IApiClient _apiClient;
        private readonly IMapper _mapper;

        public EmployeeUserService(IApiClient apiClient, IMapper mapper)
        {
            _apiClient = apiClient;
            _mapper = mapper;
        }

        public async Task<IEnumerable<Employee>> GetEmployeesWithUserAccountsAsync()
        {
            try
            {
                var responseUsersResult = await _apiClient.GetAsync<OperationResult<IEnumerable<UserAccountDto>>>("user/all");
                if (!responseUsersResult.IsSuccessful)
                {
                    Debug.WriteLine("Error fetching user accounts");
                    return Enumerable.Empty<Employee>();
                }

                var userAccounts = responseUsersResult.Data;

                var allEmployeesResult = await _apiClient.GetAsync<OperationResult<IEnumerable<EmployeeDto>>>("employee/all");
                if (!allEmployeesResult.IsSuccessful)
                {
                    Debug.WriteLine("Error fetching employees");
                    return Enumerable.Empty<Employee>();
                }

                var employeeDtos = allEmployeesResult.Data;

                var employeesWithMappedAccounts = employeeDtos
                    .Where(empDto => userAccounts.Any(ua => ua.EmployeeId == empDto.EmployeeId)) // Ensure there's a matching UserAccount
                    .Select(empDto =>
                    {
                        var employee = _mapper.Map<Employee>(empDto);
                        var userAccount = userAccounts.FirstOrDefault(ua => ua.EmployeeId == empDto.EmployeeId);
                        _mapper.Map(userAccount, employee); 
                        return employee;
                    }).ToList();

                return employeesWithMappedAccounts;
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                return Enumerable.Empty<Employee>();
            }
        }

    }
}
