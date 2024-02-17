using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using TCMS.Data.Models;

namespace TCMS.Common.DataAnnotations
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class ValidRoleAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            var roles = RoleHelpers.GetAllRoles();
            if (value is string roleValue)
            {
                return roles.Contains(roleValue);
            }
            return false;
        }

        protected override ValidationResult? IsValid(object value, ValidationContext validationContext)
        {
            return !IsValid(value) ? new ValidationResult(ErrorMessage ?? "Invalid role.") : ValidationResult.Success;
        }
    }
}

