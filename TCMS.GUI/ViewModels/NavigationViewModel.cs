using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

        public ICommand HomeCommand { get; set; }
        public ICommand EmployeesCommand { get; set; }
        public ICommand ProductsCommand { get; set; }
        //public ICommand OrdersCommand { get; set; }
        public ICommand TimeClockCommand { get; set; }
        public ICommand ShipmentsCommand { get; set; }
        public ICommand SettingsCommand { get; set; }
        public ICommand IncidentsCommand { get; set; }

        private void Home(object obj) => CurrentView = _viewModelFactory.CreateHomeViewModel();
        private void employee(object obj) => CurrentView = _viewModelFactory.CreateEmployeeViewModel();
        private void Product(object obj) => CurrentView = _viewModelFactory.CreateProductsViewModel();
        //private void Order(object obj) => CurrentView = new OrderVM();
        private void TimeClock(object obj) => CurrentView = _viewModelFactory.CreateTimeClockViewModel();   
        private void Shipment(object obj) => CurrentView = _viewModelFactory.CreateShipmentsViewModel();
        private void Setting(object obj) => CurrentView = _viewModelFactory.CreateSettingsViewModel();
        private void Incident(object obj) => CurrentView = _viewModelFactory.CreateIncidentLogViewModel();

        public NavigationViewModel(IViewModelFactory viewModelFactory)
        {
            _viewModelFactory = viewModelFactory;
            HomeCommand = new RelayCommand(Home);
            EmployeesCommand = new RelayCommand(employee);
            ProductsCommand = new RelayCommand(Product);
            //OrdersCommand = new RelayCommand(Order);
            TimeClockCommand = new RelayCommand(TimeClock);
            ShipmentsCommand = new RelayCommand(Shipment);
            SettingsCommand = new RelayCommand(Setting);
            IncidentsCommand = new RelayCommand(Incident);

            // Startup Page
            CurrentView = new HomeViewModel();
        }
    }
}
