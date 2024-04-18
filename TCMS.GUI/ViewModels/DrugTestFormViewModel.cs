using AutoMapper;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows;
using TCMS.Common.DTOs.Employee;
using TCMS.Common.DTOs.Incident;
using TCMS.Common.Operations;
using TCMS.GUI.Services.Interfaces;
using TCMS.GUI.Utilities;
using System.ComponentModel;
using TCMS.GUI.Models;
using TCMS.GUI.Views;


namespace TCMS.GUI.ViewModels
{
    class DrugTestFormViewModel : ViewModelBase, IDialogRequestClose, INotifyPropertyChanged
    {
        private readonly IApiClient _apiClient;
        private readonly IMapper _mapper;

        private DrugTest _currentDrugTest;
        private ObservableCollection<Employee> _employees;
        private ObservableCollection<Employee> _filteredEmployees;
        private Employee _selectedEmployee;
        public ObservableCollection<Employee> FilteredEmployees
        {
            get => _filteredEmployees;
            set
            {
                _filteredEmployees = value;
                OnPropertyChanged();
            }
        }
        private string _filterString;

        public string FilterString
        {
            get => _filterString;
            set
            {
                if (_filterString != value)
                {
                    _filterString = value;
                    OnPropertyChanged();
                    FilterEmployees(value);
                }
            }
        }

        public ObservableCollection<string> Items { get; set; }
        private string _selectedItem;

        public string SelectedItem
        {
            get => _selectedItem;
            set
            {
                if (_selectedItem != value)
                {
                    _selectedItem = value;
                    OnPropertyChanged(nameof(SelectedItem));
                }
            }
        }

        public Employee SelectedEmployee
        {
            get => _selectedEmployee;
            set
            {
                _selectedEmployee = value;
                OnPropertyChanged();
            }
        }

        private async Task LoadEmployees()
        {
            try
            {
                var allEmployeesResult = await _apiClient.GetAsync<OperationResult<IEnumerable<EmployeeDto>>>("employee/all");
                if (allEmployeesResult.IsSuccessful)
                {
                    var employees = _mapper.Map<IEnumerable<Employee>>(allEmployeesResult.Data);
                    App.Current.Dispatcher.Invoke(() =>
                    {
                        _employees.Clear();
                        foreach (var employee in employees)
                        {
                            _employees.Add(employee);
                        }
                        FilterEmployees(FilterString);
                    });
                }
                else
                {
                    Debug.WriteLine("Failed to load employees or no employees returned.");
                }

            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
            }
        }
        private void FilterEmployees(string filterInput)
        {
            if (string.IsNullOrEmpty(filterInput))
            {
                if (_filteredEmployees.Count != _employees.Count)
                {
                    _filteredEmployees.Clear();
                    foreach (var employee in _employees)
                    {
                        _filteredEmployees.Add(employee);
                    }
                }
            }
            else
            {
                _filteredEmployees.Clear();
                var lowerCaseFilter = filterInput.ToLower();
                foreach (var employee in _employees.Where(e => e.FullName.ToLower().Contains(lowerCaseFilter)))
                {
                    _filteredEmployees.Add(employee);
                }
            }
        }


        public event EventHandler DrugTestUpdated;

        protected virtual void OnDrugTestUpdated()
        {
            DrugTestUpdated?.Invoke(this, EventArgs.Empty);
        }
        public Action CloseAction { get; set; }

        private bool _isFatal;

        public bool IsFatal
        {
            get => _isFatal;
            set
            {
                if (_isFatal != value)
                {
                    _isFatal = value;
                    OnPropertyChanged(nameof(IsFatal));
                }
            }
        }
 
        public DrugTest CurrentDrugTest
        {
            get => _currentDrugTest;
            set
            {
                if (_currentDrugTest != value)
                {
                    _currentDrugTest = value;
                    OnPropertyChanged(nameof(CurrentDrugTest));
                }
            }
        }

        private bool _isEditMode;

        public bool IsEditMode
        {
            get => _isEditMode;
            set
            {
                if (_isEditMode != value)
                {
                    _isEditMode = value;
                    WindowTitle = value ? "Edit Product" : "Add Product";
                    OnPropertyChanged(nameof(IsEditMode));
                }
            }
        }

        private string _incidentReportId = "Enter Vehicle ID...";
        public string IncidentReportId
        {
            get => string.IsNullOrEmpty(_incidentReportId) ? "Name" : _incidentReportId;
            set
            {
                _incidentReportId = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(NamePlaceholderVisible));
            }
        }
        private DateTime? _selectedDate;
        public DateTime? SelectedDate
        {
            get => _selectedDate;
            set
            {
                _selectedDate = value;
                OnPropertyChanged();
            }
        }
        private int _vehicleId;
        public int VehicleId
        {
            get => _vehicleId;
            set
            {
                _vehicleId = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(NamePlaceholderVisible));
            }
        }

        private string _employeeId = "Enter Employee ID...";
        public string EmployeeId
        {
            get => string.IsNullOrEmpty(_employeeId) ? "Name" : _employeeId;
            set
            {
                _employeeId = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(NamePlaceholderVisible));
            }
        }

        private string _location = "Enter Location...";
        public string Location
        {
            get => string.IsNullOrEmpty(_location) ? "Name" : _location;
            set
            {
                _location = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(NamePlaceholderVisible));
            }
        }


        private string _description = "Enter Incident description...";
        public string Description
        {
            get => string.IsNullOrEmpty(_description) ? "MiddleName" : _description;
            set
            {
                _description = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(DescriptionPlaceholderVisible));
            }
        }
        private string _incidentType1 = "Accident";
        public string IncidentType1
        {
            get => string.IsNullOrEmpty(_incidentType1) ? "IncidentType" : _incidentType1;
            set
            {
                _incidentType1 = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(DescriptionPlaceholderVisible));
            }
        }
        private string _incidentType2 = "SafetyViolation";
        public string IncidentType2
        {
            get => string.IsNullOrEmpty(_incidentType2) ? "IncidentType" : _incidentType2;
            set
            {
                _incidentType2 = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(DescriptionPlaceholderVisible));
            }
        }
        private string _incidentType3 = "Other";
        public string IncidentType3
        {
            get => string.IsNullOrEmpty(_incidentType3) ? "IncidentType" : _incidentType3;
            set
            {
                _incidentType3 = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(DescriptionPlaceholderVisible));
            }
        }

        private string _windowTitle = "Add Product";

        public event EventHandler<DialogCloseRequestedEventArgs> CloseRequested;

        public string WindowTitle
        {
            get => _windowTitle;
            set
            {
                _windowTitle = value;
                OnPropertyChanged();
            }
        }

        public Visibility NamePlaceholderVisible => string.IsNullOrEmpty(_incidentReportId) ? Visibility.Visible : Visibility.Collapsed;
        public Visibility DescriptionPlaceholderVisible => string.IsNullOrEmpty(_description) ? Visibility.Visible : Visibility.Collapsed;
        public Visibility PricePlaceholderVisible => string.IsNullOrEmpty(_location) ? Visibility.Visible : Visibility.Collapsed;

        public ICommand ConfirmCommand { get; }

        public DrugTestFormViewModel(IApiClient apiClient, IMapper mapper, DrugTest drugtest = null)
        {
            Items = new ObservableCollection<string> { "Accident", "SafetyViolation", "Other" };

            _apiClient = apiClient;
            _mapper = mapper;
            IsEditMode = false;

            if (drugtest != null)
            {
                CurrentDrugTest = drugtest;
                IsEditMode = true;
            }
            else
            {
                CurrentDrugTest = new DrugTest();
                IsEditMode = false;
            }

            if (IsEditMode)
            {
            }
            ConfirmCommand = new RelayCommand(Confirm);
            _employees = new ObservableCollection<Employee>();
            _filteredEmployees = new ObservableCollection<Employee>();
            _ = LoadEmployees();

        }
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        private async void Confirm(object obj)
        {
            if (IsEditMode)
            {
                await UpdateIncidentAsync();
            }
            else
            {
                await AddIncidentAsync();
            }

            // Close the window or navigate back as appropriate
            CloseRequested?.Invoke(this, new DialogCloseRequestedEventArgs(true));
        }

        private async Task AddIncidentAsync()
        {
            try
            {
                var newIncidentReportDto = _mapper.Map<IncidentReportDto>(this);

                var result = await _apiClient.PostAsync<OperationResult>("incident/create", newIncidentReportDto);
                if (!result.IsSuccessful)
                {
                    Debug.WriteLine(result.Messages);
                }
                else if (result.IsSuccessful)
                {
                    DrugTestUpdated?.Invoke(this, EventArgs.Empty);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }

        private async Task UpdateIncidentAsync()
        {
            try
            {

                var updatedDrugTestDto = _mapper.Map<DrugTest>(this);
                updatedDrugTestDto.IncidentReportId = CurrentDrugTest.IncidentReportId;

                var result = await _apiClient.PutAsync<OperationResult>("drug-test/all", updatedDrugTestDto);
                if (!result.IsSuccessful)
                {
                    Debug.WriteLine(result.Messages);
                }
                else if (result.IsSuccessful)
                {
                    DrugTestUpdated?.Invoke(this, EventArgs.Empty);
                }

            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }

        public void Cleanup()
        {
            DrugTestUpdated = null;
        }
    }
}

