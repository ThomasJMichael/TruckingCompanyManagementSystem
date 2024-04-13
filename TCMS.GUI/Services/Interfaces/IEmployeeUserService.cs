using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TCMS.GUI.Models;

namespace TCMS.GUI.Services.Interfaces
{
    public interface IEmployeeUserService
    {
        Task<IEnumerable<Employee>> GetEmployeesWithUserAccountsAsync();
    }

}
