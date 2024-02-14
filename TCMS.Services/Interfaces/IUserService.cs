using TCMS.Common.DTOs.User;
using TCMS.Data.Models;

namespace TCMS.Services.Interfaces;

public interface IUserService
{
    // Account Management
    Task<bool> ChangePassword(ChangePasswordDto changePasswordDto);
    Task<UserAccount> CreateUserAccountAsync(NewAccountDto newAccountDto);
    Task<bool> UpdateUserAccountAsync(UpdateAccountDto updateAccountDto);
    Task<bool> DeleteUserAccountAsync(int id);

    // User Management
    Task<bool> UpdatePayRate (int userId, decimal newPayRate);

    // Role and Permission Management
    Task<bool> AddRoleToUserAsync(int userId, string roleName);
    Task<bool> RemoveRoleFromUserAsync(int userId, string roleName);
    Task<IEnumerable<Role>> GetRolesAsync(int userId);

    // Employee and Driver Account Linking
    Task<bool> LinkEmployeeToUserAccountAsync (int employeeId, int userId);

    // Account information retrieval
    Task<UserAccountDto> GetUserAccountByIdAsync(int id);
    Task<IEnumerable<UserAccountDto>> GetAllUserAccountsAsync();

}