using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TCMS.Data.Models
{
    public class Payroll
    {
        public int PayrollId { get; set; }
        public string EmployeeId { get; set; }
        public virtual Employee Employee { get; set; }
        
        public DateTime PayPeriodStart { get; set; }
        public DateTime PayPeriodEnd { get; set; }
        public decimal GrossPay { get; set; }
        public decimal TaxDeductions { get; set; }
        public decimal OtherDeductions { get; set; }
        public decimal NetPay { get; private set; }

        // Additional properties for further detail if required
        public decimal OvertimePay { get; set; }
        public decimal RegularPay { get; set; }

        // Information about specific deductions
        public decimal SocialSecurityTax { get; set; }
        public decimal MedicareTax { get; set; }
        public decimal StateIncomeTax { get; set; }
        public decimal FederalIncomeTax { get; set; }
        // Any other deductions

        // Calculate net pay
        public void CalculateNetPay()
        {
            NetPay = GrossPay - TaxDeductions - OtherDeductions;
        }
        // Calculate tax deductions
        public void CalculateTaxDeductions()
        {
            TaxDeductions = SocialSecurityTax + MedicareTax + StateIncomeTax + FederalIncomeTax;
        }
        // Calculate gross pay
        public void CalculateGrossPay()
        {
            GrossPay = OvertimePay + RegularPay;
        }
        // Validate payroll data
        public bool Validate()
        {
            var isValid = !(PayPeriodStart > PayPeriodEnd);
            if (GrossPay < 0) isValid = false;
            if (TaxDeductions < 0) isValid = false;
            if (OtherDeductions < 0) isValid = false;
            if (NetPay < 0) isValid = false;
            if (OvertimePay < 0) isValid = false;
            if (RegularPay < 0) isValid = false;
            if (SocialSecurityTax < 0) isValid = false;
            if (MedicareTax < 0) isValid = false;
            if (StateIncomeTax < 0) isValid = false;
            if (FederalIncomeTax < 0) isValid = false;
            return isValid;
        }
    }
}
