namespace TCMS.Common.DTOs.User
{
    public class ChangePasswordDto
    {
        public int EmployeeId { get; set; }
        public string OldPassword { get; set; }
        public string NewPassword { get; set; }
    }
}
