using System.ComponentModel.DataAnnotations;
using TCMS.Common.DataAnnotations;

namespace TCMS.Common.DTOs.User
{
    public class NewAccountDto
    {
        [Required]
        [StringLength(50, MinimumLength = 3)]
        public string Username { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        public string Role { get; set; }

        [Required]
        [StringLength(50)]
        public string FirstName { get; set; }

        [StringLength(50)] public string? MiddleName { get; set; } = "";

        [Required]
        [StringLength(50)]
        public string LastName { get; set; }

        [Required]
        [StringLength(50)]
        public string Address { get; set; }

        [Required]
        [StringLength(50)]
        public string City { get; set; }

        [Required]
        [StringLength(2)]
        public string State { get; set; }

        [Required]
        [StringLength(5)]
        public string Zip { get; set; }

        [Phone(ErrorMessage = "Invalid Phone Number.")]
        [RegularExpression(@"^\+?(\d{1,3})?[-. ]?(\(\d{1,3}\)|\d{1,3})?[-. ]?\d{1,4}[-. ]?\d{1,4}[-. ]?\d{1,9}$",
            ErrorMessage = "Invalid Phone Number Format.")]
        public string? HomePhoneNumber { get; set; } = "";

        [Phone(ErrorMessage = "Invalid Phone Number.")]
        [RegularExpression(@"^\+?(\d{1,3})?[-. ]?(\(\d{1,3}\)|\d{1,3})?[-. ]?\d{1,4}[-. ]?\d{1,4}[-. ]?\d{1,9}$",
            ErrorMessage = "Invalid Phone Number Format.")]
        public string? CellPhoneNumber { get; set; } = "";

        [Required(ErrorMessage = "Start Date is required.")]
        [DataType(DataType.Date, ErrorMessage = "Invalid Date Format.")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime StartDate { get; set; }

        [Required]
        [Range(0, double.MaxValue, ErrorMessage = "Please enter valid pay rate")]
        public decimal PayRate { get; set; }
        public DriverDetails? DriverInfo { get; set; }

    }

    public class DriverDetails
    {
        [Required(ErrorMessage = "CDL number is required.")]
        [RegularExpression("^[A-Z0-9]+$", ErrorMessage = "Invalid CDL number format.")]
        public string CDLNumber { get; set; }

        [Required(ErrorMessage = "CDL Expiration Date is required.")]
        [DataType(DataType.Date)]
        [DateInTheFuture(ErrorMessage = "CDL Expiration Date must be in the future.")]
        public DateTime CDLExperationDate { get; set; }
    }
}

