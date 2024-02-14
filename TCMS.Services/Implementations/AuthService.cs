using TCMS.Common.DTOs.User;
using TCMS.Services.Interfaces;

namespace TCMS.Services.Implementations;

public class AuthService : IAuthService
{
    Task<bool> IAuthService.AuthenticateAsync(LoginDto loginDto)
    {
        throw new NotImplementedException();
    }
}
