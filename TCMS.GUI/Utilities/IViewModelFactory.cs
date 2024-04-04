using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TCMS.GUI.ViewModels;

namespace TCMS.GUI.Utilities
{
    public interface IViewModelFactory
    {
        HomeViewModel CreateHomeViewModel();
        EmployeeViewModel CreateEmployeeViewModel();
        ProductsViewModel CreateProductsViewModel();
        TimeClockViewModel CreateTimeClockViewModel();
        ShipmentsViewModel CreateShipmentsViewModel();
        SettingsViewModel CreateSettingsViewModel();
        ProductFormViewModel CreateProductFormViewModel();
    }

}
