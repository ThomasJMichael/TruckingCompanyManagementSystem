using TCMS.Data.Models;

namespace TCMS.Common.DTOs.User
{
    public class UserAccountDto
    {
        public string EmployeeId { get; set; }
        public string Username { get; set; }
        public string? UserRole { get; set; }

    }
}
