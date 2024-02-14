namespace TCMS.Common.DTOs.User
{
    public class UpdateAccountDto
    {
        public string? Username { get; set; }
        public string? Password { get; set; }
        public string? FirstName { get; set; }
        public string? MiddleName { get; set; }
        public string? LastName { get; set; }
        public string? Address { get; set; }
        public string? City { get; set; }
        public string? State { get; set; }
        public string? Zip { get; set; }
        public string? HomePhoneNumber { get; set; }
        public string? CellPhoneNumber { get; set; }
        public decimal? PayRate { get; set; }
        public DriverDetails? DriverInfo { get; set; }
    }
}
