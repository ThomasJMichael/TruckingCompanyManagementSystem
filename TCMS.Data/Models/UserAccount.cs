using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TCMS.Data.Models
{
    public enum DataType
    {
        Equipment,
        Shipping,
        Maintenance,
    }

    public enum Role
    {
        Full,
        Shipping,
        Maintenance,
        Driver,
    }
    public class UserAccount
    {
        public int UserAccountId { get; set; }
        public string Username { get; set; }
        public string PasswordHash { get; set; }
        public Role UserRole { get; set; }

        public int? EmployeeId { get; set; }
        public virtual Employee Employee { get; set; }

        public bool HasAccessTo(DataType dataType)
        {
            switch (UserRole)
            {
                case Role.Full:
                    return true;
                case Role.Shipping:
                    return dataType is DataType.Shipping or DataType.Equipment or DataType.Maintenance;
                case Role.Maintenance:
                    return dataType is DataType.Maintenance or DataType.Equipment;
                case Role.Driver:
                    // Need to handle driver only having access to their specific shipments.
                    return dataType == DataType.Shipping;
                default:
                    return false;
            }
        }
    }
}
