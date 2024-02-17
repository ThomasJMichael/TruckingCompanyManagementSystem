using TCMS.Common.DTOs.Employee;
using TCMS.Common.DTOs.User;
using TCMS.Common.Operations;

namespace TCMS.Services.Interfaces;

public interface IUserService
{
    // Account Management
    Task<OperationResult> CreateUserAccountAsync(NewAccountDto newAccountDto);
    Task<OperationResult> UpdateUsernameAsync(ChangeUsernameDto changeUsernameDto);
    Task<OperationResult> DeleteUserAccountAsync(string userId);

    // User Management
    Task<OperationResult> UpdatePayRate(UpdatePayRateDto updatePayRateDto);

    // Role and Permission Management
    Task<OperationResult> AddRoleToUserAsync(UserRoleDto userRoleDto);
    Task<OperationResult> RemoveRoleFromUserAsync(UserRoleDto userRoleDto);
    Task<OperationResult<IEnumerable<string>>> GetRolesAsync(string userId);

    // Account information retrieval
    Task<OperationResult<UserAccountDto>> GetUserAccountByIdAsync(string id);
    Task<OperationResult<IEnumerable<UserAccountDto>>> GetAllUserAccountsAsync();

}
