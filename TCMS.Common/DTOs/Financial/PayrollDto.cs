using System.ComponentModel.DataAnnotations;

namespace TCMS.Common.DTOs.Financial
{
    public class PayrollDto
    {
        public int PayrollId { get; set; }

        [Required]
        public string EmployeeId { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime PayPeriodStart { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime PayPeriodEnd { get; set; }

        [Required]
        [Range(0, double.MaxValue, ErrorMessage = "Gross Pay must be positive.")]
        public decimal GrossPay { get; set; }

        [Required]
        [Range(0, double.MaxValue, ErrorMessage = "Tax Deductions must be positive.")]
        public decimal TaxDeductions { get; set; }

        [Required]
        [Range(0, double.MaxValue, ErrorMessage = "Other Deductions must be positive.")]
        public decimal OtherDeductions { get; set; }

        // NetPay is calculated based on other properties, so it's not directly settable
        public decimal NetPay { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "Overtime Pay must be positive.")]
        public decimal OvertimePay { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "Regular Pay must be positive.")]
        public decimal RegularPay { get; set; }

        // Additional fields for deductions can be added with similar validation
        [Range(0, double.MaxValue, ErrorMessage = "Social Security Tax must be positive.")]
        public decimal SocialSecurityTax { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "Medicare Tax must be positive.")]
        public decimal MedicareTax { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "State Income Tax must be positive.")]
        public decimal StateIncomeTax { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "Federal Income Tax must be positive.")]
        public decimal FederalIncomeTax { get; set; }
    }
}
