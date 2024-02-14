using TCMS.Common.DTOs.User;

namespace TCMS.Services.Interfaces;

public interface IAuthService
{
    Task<bool> AuthenticateAsync(LoginDto loginDto);

}