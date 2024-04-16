using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using TCMS.Data.Models;
using TCMS.GUI.Utilities;

namespace TCMS.GUI.ViewModels
{
    public class NavigationViewModel : ViewModelBase
    {
        private object _currentView;
        private readonly IViewModelFactory _viewModelFactory;
        public object CurrentView
        {
            get { return _currentView; }
            set { _currentView = value; OnPropertyChanged(); }
        }

        private string _userRole;

        public string UserRole
        {
            get { return _userRole; }
            set
            {
                _userRole = value;
                OnPropertyChanged();
                UpdateCommandVisibility();
            }
        }

        private void UpdateCommandVisibility()
        {
            CommandManager.InvalidateRequerySuggested();
        }

        public ICommand HomeCommand { get; set; }
        public ICommand EmployeesCommand { get; set; }
        public ICommand ProductsCommand { get; set; }
        public ICommand OrdersCommand { get; set; }
        public ICommand TimeClockCommand { get; set; }
        public ICommand ShipmentsCommand { get; set; }
        public ICommand EquipmentCommand { get; set; }
        public ICommand IncidentsCommand { get; set; }
        public ICommand DrugTestsCommand { get; set; }

        public ICommand AssignmentCommand { get; set; } 
        public ICommand ReportCommand { get; set; }

        private void Home(object obj) => CurrentView = _viewModelFactory.CreateHomeViewModel();
        private void Assignment(object ob) => CurrentView = _viewModelFactory.CreateAssignmentsViewModel();
        private void employee(object obj) => CurrentView = _viewModelFactory.CreateEmployeeViewModel();
        private void Product(object obj) => CurrentView = _viewModelFactory.CreateProductsViewModel();
        private void Order(object obj) => CurrentView = new OrderViewModel();
        private void TimeClock(object obj) => CurrentView = _viewModelFactory.CreateTimeClockViewModel();   
        private void Shipment(object obj) => CurrentView = _viewModelFactory.CreateShipmentsViewModel();
        private void Equipment(object obj) => CurrentView = _viewModelFactory.CreateEquipmentViewModel();
        private void Incident(object obj) => CurrentView = _viewModelFactory.CreateIncidentLogViewModel();
        private void DrugTest(object obj) => CurrentView = _viewModelFactory.CreateDrugTestViewModel();
        private void Report(object obj) => CurrentView = _viewModelFactory.CreateReportViewModel();

        public NavigationViewModel(IViewModelFactory viewModelFactory)
        {
            _viewModelFactory = viewModelFactory;
            HomeCommand = new RelayCommand(Home);
            AssignmentCommand = new RelayCommand(Assignment);
            EmployeesCommand = new RelayCommand(employee);
            ProductsCommand = new RelayCommand(Product);
            OrdersCommand = new RelayCommand(Order);
            TimeClockCommand = new RelayCommand(TimeClock);
            ShipmentsCommand = new RelayCommand(Shipment);
            EquipmentCommand = new RelayCommand(Equipment);
            IncidentsCommand = new RelayCommand(Incident);
            DrugTestsCommand = new RelayCommand(DrugTest);
            ReportCommand = new RelayCommand(Report);
            UserRole = App.Current.Properties["UserRole"] as string;

            // Startup Page
            CurrentView = new HomeViewModel();
        }
        public Visibility EmployeesVisibility => IsVisibleForRole("Employee") ? Visibility.Visible : Visibility.Collapsed;
        public Visibility ProductsVisibility => IsVisibleForRole("Products") ? Visibility.Visible : Visibility.Collapsed;
        public Visibility OrdersVisibility => IsVisibleForRole("Orders") ? Visibility.Visible : Visibility.Collapsed;
        public Visibility ShipmentsVisibility => IsVisibleForRole("Shipments") ? Visibility.Visible : Visibility.Collapsed;
        public Visibility TimeClockVisibility => IsVisibleForRole("TimeClock") ? Visibility.Visible : Visibility.Collapsed;
        public Visibility AssignmentsVisibility => IsVisibleForRole("Assignments") ? Visibility.Visible : Visibility.Collapsed;
        public Visibility IncidentsVisibility => IsVisibleForRole("Incidents") ? Visibility.Visible : Visibility.Collapsed;
        public Visibility EquipmentVisibility => IsVisibleForRole("Equipment") ? Visibility.Visible : Visibility.Collapsed;
        public Visibility DrugTestsVisibility => IsVisibleForRole("DrugTests") ? Visibility.Visible : Visibility.Collapsed;
        public Visibility ReportVisibility => IsVisibleForRole("Report") ? Visibility.Visible : Visibility.Collapsed;

        private bool IsVisibleForRole(string featureName)
        {
            switch (featureName)
            {
                case "Home":
                    return true;
                case "Employee":
                    return UserRole is "Admin";
                case "DrugTests":
                    return UserRole is "Admin";
                case "Products":
                    return UserRole is "Admin" or "ShippingManager";
                case "Orders":
                    return UserRole is "Admin" or "ShippingManager";
                case "TimeClock":
                    return true;
                case "Report":
                    return UserRole is "Admin";
                case "Shipments":
                    return UserRole is "Admin" or "ShippingManager";
                case "Assignments":
                    return UserRole is "Admin" or "Driver";
                case "Incidents":
                    return UserRole is "Admin";
                case "Equipment":
                    return UserRole is "Admin" or "MaintenanceWorker";
                default:
                    return false;
            }
        }
    }
}
