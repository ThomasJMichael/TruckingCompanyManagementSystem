using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using TCMS.Data.Models;
using TCMS.GUI.ViewModels;

namespace TCMS.GUI.Utilities
{
    public class ViewModelFactory : IViewModelFactory
    {
        private readonly IServiceProvider _serviceProvider;

        public ViewModelFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public HomeViewModel CreateHomeViewModel() => _serviceProvider.GetRequiredService<HomeViewModel>();
        public ShipmentsViewModel CreateShipmentsViewModel() => _serviceProvider.GetRequiredService<ShipmentsViewModel>();
        public EmployeeViewModel CreateEmployeeViewModel() => _serviceProvider.GetRequiredService<EmployeeViewModel>();
        public ProductsViewModel CreateProductsViewModel() => _serviceProvider.GetRequiredService<ProductsViewModel>();
        public TimeClockViewModel CreateTimeClockViewModel() => _serviceProvider.GetRequiredService<TimeClockViewModel>();
        public ProductFormViewModel CreateProductFormViewModel() => _serviceProvider.GetRequiredService<ProductFormViewModel>();
        public IncidentLogViewModel CreateIncidentLogViewModel() => _serviceProvider.GetRequiredService<IncidentLogViewModel>();

        public OrderViewModel CreateOrderViewModel() => _serviceProvider.GetRequiredService<OrderViewModel>();
        public EquipmentViewModel CreateEquipmentViewModel() => _serviceProvider.GetRequiredService<EquipmentViewModel>();

        public AssignmentsViewModel CreateAssignmentsViewModel() =>
            _serviceProvider.GetRequiredService<AssignmentsViewModel>();
        public DrugTestViewModel CreateDrugTestViewModel() => _serviceProvider.GetRequiredService<DrugTestViewModel>();

        public ReportViewModel CreateReportViewModel() => _serviceProvider.GetRequiredService<ReportViewModel>();
    }
}
