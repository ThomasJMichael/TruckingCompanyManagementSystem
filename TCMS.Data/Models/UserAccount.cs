using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace TCMS.Data.Models
{
    public static class Role
    {
        public const string Admin = "Admin";
        public const string ShippingManager = "ShippingManager";
        public const string MaintenanceWorker = "MaintenanceWorker";
        public const string Driver = "Driver";
        public const string Default = "Default";
    }
    public static class RoleHelpers
    {
        public static IEnumerable<string?> GetAllRoles()
        {
            return typeof(Role)
                .GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy)
                .Where(fi => fi is { IsLiteral: true, IsInitOnly: false })
                .Select(selector: fi => fi.GetValue(null)?.ToString());
        }
    }
    public class UserAccount : IdentityUser
    {
        public string? EmployeeId { get; set; }
        public virtual Employee Employee { get; set; }
    }
}
