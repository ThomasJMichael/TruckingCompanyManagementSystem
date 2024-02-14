using TCMS.Data.Models;

namespace TCMS.Common.DTOs.User
{
    public class UserAccountDto
    {
        public int UserAccountId { get; set; }
        public string Username { get; set; }
        public Role UserRole { get; set; }
        public int EmployeeId { get; set; }

    }
}
