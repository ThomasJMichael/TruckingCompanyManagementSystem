﻿using Microsoft.AspNetCore.Identity;
using TCMS.Common.DTOs.User;
using TCMS.Common.Operations;
using TCMS.Data.Models;
using TCMS.Services.Interfaces;

namespace TCMS.Services.Implementations;

public class AuthService(UserManager<UserAccount> userManager, SignInManager<UserAccount> signInManager)
    : IAuthService
{
    public async Task<OperationResult<UserAccountDto>> LoginAsync(LoginDto loginDto)
    {
        var user = await userManager.FindByNameAsync(loginDto.Username);
        if (user == null) return OperationResult<UserAccountDto>.Failure(["Failed to find user."]);
        var result = await signInManager.PasswordSignInAsync(user, loginDto.Password, false, false);
        if (result.Succeeded)
        {
            var userRole = await userManager.GetRolesAsync(user);
            return OperationResult<UserAccountDto>.Success(new UserAccountDto
            {
                Username = user.UserName,
                EmployeeId = user.EmployeeId,
                UserRole = userRole.FirstOrDefault()
            });
        }
        return OperationResult<UserAccountDto>.Failure(["Failed to login."]);
    }
    
    public async Task<OperationResult> LogoutAsync()
    {
        await signInManager.SignOutAsync();
        return OperationResult.Success();
    }

    public async Task<OperationResult> ChangePasswordAsync(ChangePasswordDto changePasswordDto)
    {
        var user = await userManager.FindByNameAsync(changePasswordDto.Username);
        if (user == null) return OperationResult.UserNotFound();

        if (changePasswordDto.NewPassword != changePasswordDto.ConfirmPassword) return OperationResult.PasswordsDoNotMatch();

        var result = await userManager.ChangePasswordAsync(user, changePasswordDto.OldPassword, changePasswordDto.NewPassword);
        return !result.Succeeded ? OperationResult.Failure(result.Errors.Select(e => e.Description)) 
                                 : OperationResult.Success();
    }
}
