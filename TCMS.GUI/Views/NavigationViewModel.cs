using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using TCMS.Data.Models;
using TCMS.GUI.Utilities;
using TCMS.GUI.ViewModels;

namespace TCMS.GUI.Views
{
    class NavigationViewModel : ViewModelBase
    {
        private object _currentView;
        public object CurrentView
        {
            get { return _currentView; }
            set { _currentView = value; OnPropertyChanged(); }
        }

        public ICommand HomeCommand { get; set; }
        public ICommand EmployeesCommand { get; set; }
        //public ICommand ProductsCommand { get; set; }
        //public ICommand OrdersCommand { get; set; }
        public ICommand TimeClockCommand { get; set; }
        public ICommand ShipmentsCommand { get; set; }
        public ICommand SettingsCommand { get; set; }

        private void Home(object obj) => CurrentView = new HomeViewModel();
        private void employee(object obj) => CurrentView = new EmployeeViewModel();
        //private void Product(object obj) => CurrentView = new ProductVM();
        //private void Order(object obj) => CurrentView = new OrderVM();
        private void TimeClock(object obj) => CurrentView = new TimeClockViewModel();
        private void Shipment(object obj) => CurrentView = new ShipmentsViewModel();
        private void Setting(object obj) => CurrentView = new SettingsViewModel();

        public NavigationViewModel()
        {
            HomeCommand = new RelayCommand(Home);
            //EmployeesCommand = new RelayCommand(employee);
            //ProductsCommand = new RelayCommand(Product);
            //OrdersCommand = new RelayCommand(Order);
            TimeClockCommand = new RelayCommand(TimeClock);
            ShipmentsCommand = new RelayCommand(Shipment);
            SettingsCommand = new RelayCommand(Setting);

            // Startup Page
            CurrentView = new HomeViewModel();
        }
    }
}
