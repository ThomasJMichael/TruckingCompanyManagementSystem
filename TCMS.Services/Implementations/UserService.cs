using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TCMS.Common.DTOs.Employee;
using TCMS.Common.DTOs.User;
using TCMS.Common.Operations;
using TCMS.Data.Data;
using TCMS.Data.Models;
using TCMS.Services.Interfaces;

namespace TCMS.Services.Implementations
{
    public class UserService : IUserService
    {
        private readonly UserManager<UserAccount> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly SignInManager<UserAccount> _signInManager;
        private readonly TcmsContext _context;

        public async Task<OperationResult> AddRoleToUserAsync(int userId, string roleName)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user == null) return OperationResult.UserNotFound();

            var role = await _roleManager.FindByNameAsync(roleName);
            if (role == null) return OperationResult.RoleNotFound();

            var result = await _userManager.AddToRoleAsync(user, roleName);
            return result.Succeeded ? OperationResult.Success() : OperationResult.Failure(result.Errors.Select(e => e.Description));
        }

        public async Task<OperationResult> CreateUserAccountAsync(NewAccountDto newAccountDto)
        {
            //Check if user already exists
            var existingUser = await _userManager.FindByNameAsync(newAccountDto.Username);
            if (existingUser != null) return OperationResult.UserAlreadyExists();

            await using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                Employee employee;
                if (newAccountDto.DriverInfo != null)
                {
                    var driver = new Driver
                    {
                        FirstName = newAccountDto.FirstName,
                        MiddleName = newAccountDto.MiddleName,
                        LastName = newAccountDto.LastName,
                        Address = newAccountDto.Address,
                        City = newAccountDto.City,
                        State = newAccountDto.State,
                        Zip = newAccountDto.Zip,
                        HomePhoneNumber = newAccountDto.HomePhoneNumber,
                        CellPhoneNumber = newAccountDto.CellPhoneNumber,
                        PayRate = newAccountDto.PayRate,
                        StartDate = DateTime.Now,
                        CDLNumber = newAccountDto.DriverInfo.CDLNumber,
                        CDLExperationDate = newAccountDto.DriverInfo.CDLExperationDate
                    };
                    _context.Drivers.Add(driver);
                    employee = driver;
                }
                else
                {
                    employee = new Employee
                    {
                        FirstName = newAccountDto.FirstName,
                        MiddleName = newAccountDto.MiddleName,
                        LastName = newAccountDto.LastName,
                        Address = newAccountDto.Address,
                        City = newAccountDto.City,
                        State = newAccountDto.State,
                        Zip = newAccountDto.Zip,
                        HomePhoneNumber = newAccountDto.HomePhoneNumber,
                        CellPhoneNumber = newAccountDto.CellPhoneNumber,
                        PayRate = newAccountDto.PayRate,
                        StartDate = newAccountDto.StartDate
                    };
                    _context.Employees.Add(employee);
                }
                await _context.SaveChangesAsync();

                var user = new UserAccount
                {
                    UserName = newAccountDto.Username,
                    EmployeeId = employee.EmployeeId
                };
                var createResult = await _userManager.CreateAsync(user, newAccountDto.Password);
                if (!createResult.Succeeded)
                {
                    await transaction.RollbackAsync();
                    return OperationResult.Failure(createResult.Errors.Select(e => e.Description));
                }

                //Add user to role
                var roleResult = await _userManager.AddToRoleAsync(user, newAccountDto.Role);
                if (!roleResult.Succeeded)
                {
                    await transaction.RollbackAsync();
                    return OperationResult.Failure(roleResult.Errors.Select(e => e.Description));
                }

                await transaction.CommitAsync();
                return OperationResult.Success();
            }
            catch (Exception e)
            {
                await transaction.RollbackAsync();
                return OperationResult.MiscError(e.Message);
            }
        }

        public async Task<OperationResult> UpdateUsernameAsync(ChangeUsernameDto changeUsernameDto)
        {
            var user = await _userManager.FindByIdAsync(changeUsernameDto.CurrentUsername);
            if (user == null) return OperationResult.UserNotFound();
            
            var setUsernameResult = await _userManager.SetUserNameAsync(user, changeUsernameDto.NewUsername);
            return setUsernameResult.Succeeded ? OperationResult.Success() : OperationResult.Failure(setUsernameResult.Errors.Select(e => e.Description));
        }

        public async Task<OperationResult> DeleteUserAccountAsync(string userId)
        {
            throw new NotImplementedException();
        }

        public async Task<OperationResult> UpdatePayRate(UpdatePayRateDto updatePayRateDto)
        {
            await using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                var employee = await _context.Employees.FindAsync(updatePayRateDto.EmployeeId);
                if (employee == null) return OperationResult.Failure(["Employee not found."]);

                employee.PayRate = updatePayRateDto.NewPayRate;
                _context.Employees.Update(employee);
                await _context.SaveChangesAsync();
                return OperationResult.Success();
            }
            catch (Exception e)
            {
                await transaction.RollbackAsync();
                return OperationResult.Failure(new List<string> { "An error occurred while updating the pay rate.", e.Message});
            }
        }

        public async Task<OperationResult> AddRoleToUserAsync(UserRoleDto userRoleDto)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(userRoleDto.UserId);
                if (user == null) return OperationResult.UserNotFound();

                var result = await _userManager.AddToRoleAsync(user, userRoleDto.RoleName);
                return result.Succeeded
                    ? OperationResult.Success()
                    : OperationResult.Failure(result.Errors.Select(e => e.Description));
            }
            catch (Exception e)
            {
                return OperationResult.Failure([e.Message]);
            }
        }

        public async Task<OperationResult> RemoveRoleFromUserAsync(UserRoleDto userRoleDto)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(userRoleDto.UserId);
                if (user == null) return OperationResult.UserNotFound();
                
                var result = await _userManager.RemoveFromRoleAsync(user, userRoleDto.RoleName);
                if (!result.Succeeded) return OperationResult.Failure(result.Errors.Select(e => e.Description));
                
                var roles = await _userManager.GetRolesAsync(user);
                if (roles.Count == 0)
                {
                    var defaultRole = await _userManager.AddToRoleAsync(user, Role.Default);
                    if (!defaultRole.Succeeded)
                        return OperationResult.Failure(defaultRole.Errors.Select(e => e.Description));
                }
                return OperationResult.Success();
            }
            catch (Exception e)
            {
                return OperationResult.MiscError(e.Message);
            }
        }

        //TODO I think this operation will fail currently because of OnDelete behavior
        public async Task<OperationResult> DeleteUserAccountAsync(int id)
        {
            var user = await _userManager.FindByIdAsync(id.ToString());
            if (user == null) return OperationResult.UserNotFound();
            
            var result = await _userManager.DeleteAsync(user);
            return result.Succeeded ? OperationResult.Success() : OperationResult.Failure(result.Errors.Select(e => e.Description));

        }

        public async Task<OperationResult<UserAccountDto>> GetUserAccountByIdAsync(string userId)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(userId);
                if (user == null) return OperationResult<UserAccountDto>.Failure(["User not found."]);

                var roles = await _userManager.GetRolesAsync(user);
                var roleName = roles.FirstOrDefault();

                var userAccountDto = new UserAccountDto
                {
                    EmployeeId = user.EmployeeId,
                    Username = user.UserName,
                    UserRole = roleName,
                };
                return OperationResult<UserAccountDto>.Success(userAccountDto);
            }
            catch (Exception e)
            {
                return OperationResult<UserAccountDto>.Failure([e.Message]);
            }
        }

    public async Task<OperationResult<IEnumerable<UserAccountDto>>> GetAllUserAccountsAsync()
        {
            try
            {
                var userAccounts = await _context.UserAccounts.ToListAsync();
                var userAccountDtos = userAccounts.Select(u => new UserAccountDto
                {
                    EmployeeId = u.EmployeeId,
                    Username = u.UserName,
                    UserRole = _roleManager.Roles.FirstOrDefault(r => r.Id == _userManager.GetRolesAsync(u).Result.FirstOrDefault())?.Name,
                }).ToList();
                return OperationResult<IEnumerable<UserAccountDto>>.Success(userAccountDtos);
            }
            catch (Exception e)
            {
                return OperationResult<IEnumerable<UserAccountDto>>.Failure([e.Message]);
            }
        }

        public async Task<OperationResult<IEnumerable<string>>> GetRolesAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return OperationResult<IEnumerable<string>>.Failure(["User not found."]);
            }

            var roles = await _userManager.GetRolesAsync(user);
            return OperationResult<IEnumerable<string>>.Success(roles);
        }

    }
}
