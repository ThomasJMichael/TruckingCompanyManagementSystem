using TCMS.Common.DTOs.User;
using TCMS.Common.Operations;

namespace TCMS.Services.Interfaces;

public interface IAuthService
{
    Task<OperationResult> LoginAsync(LoginDto loginDto);
    Task<OperationResult> LogoutAsync();
    Task<OperationResult> ChangePasswordAsync(ChangePasswordDto changePasswordDto);

}