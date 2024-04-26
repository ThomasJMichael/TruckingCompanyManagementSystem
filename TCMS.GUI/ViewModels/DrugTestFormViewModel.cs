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
using TCMS.Common.enums;
using TCMS.Common.DTOs.DrugTest;


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

        private string _employeeId;
        public string EmployeeId
        {
            get => string.IsNullOrEmpty(_employeeId) ? "Name" : _employeeId;
            set
            {
                _employeeId = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(EmployeeId));
            }
        }

        public Employee SelectedEmployee
        {
            get => _selectedEmployee;
            set
            {
                _selectedEmployee = value;
                OnPropertyChanged();
                if (_selectedEmployee != null)
                {
                    EmployeeId = _selectedEmployee.EmployeeId;
                }
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

        private bool _isFollowUpComplete;

        public bool IsFollowUpComplete
        {
            get => _isFollowUpComplete;
            set
            {
                if (_isFollowUpComplete != value)
                {
                    _isFollowUpComplete = value;
                    OnPropertyChanged(nameof(IsFollowUpComplete));
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

        private int _drugAndAlcoholTestId;
        public int DrugAndAlcoholTestId
        {
            get => _drugAndAlcoholTestId;
            set
            {
                _drugAndAlcoholTestId = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(DrugAndAlcoholTestId));
            }
        }

        private int? _incidentReportId;
        public int? IncidentReportId
        {
            get => _incidentReportId;
            set
            {
                _incidentReportId = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(IncidentReportId));
            }
        }
        private DateTime _testDate;
        public DateTime TestDate
        {
            get => _testDate;
            set
            {
                _testDate = value;
                OnPropertyChanged();
            }
        }
        public Array TestTypes => Enum.GetValues(typeof(TestType));
        private TestType _testType;
        public TestType TestType
        {
            get => _testType;
            set
            {
                _testType = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(TestType));
            }
        }
        public Array TestResults => Enum.GetValues(typeof(TestResult));
        private TestResult _testResult;
        public TestResult TestResult
        {
            get => _testResult;
            set
            {
                _testResult = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(TestResult));
            }
        }

        private string _testDetails = "Enter Incident description...";
        public string TestDetails
        {
            get => string.IsNullOrEmpty(_testDetails) ? "MiddleName" : _testDetails;
            set
            {
                _testDetails = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(TestDetails));
            }
        }
        private DateTime? _followUpTestDate;
        public DateTime? FollowUpTestDate
        {
            get => _followUpTestDate;
            set
            {
                _followUpTestDate = value;
                OnPropertyChanged();
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

        //public Visibility NamePlaceholderVisible => string.IsNullOrEmpty(_drugAndAlcoholTestId) ? Visibility.Visible : Visibility.Collapsed;
        //public Visibility DescriptionPlaceholderVisible => string.IsNullOrEmpty(_testDetails) ? Visibility.Visible : Visibility.Collapsed;
        //public Visibility PricePlaceholderVisible => DateTime.IsNullOrEmpty(_followUpTestDate) ? Visibility.Visible : Visibility.Collapsed;

        public ICommand ConfirmCommand { get; }

        public DrugTestFormViewModel(IApiClient apiClient, IMapper mapper, DrugTest drugtest = null)
        {

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
                TestDate = CurrentDrugTest.TestDate;
                TestType = CurrentDrugTest.TestType;
                TestResult = CurrentDrugTest.TestResult;
                TestDetails = CurrentDrugTest.TestDetails;
                IncidentReportId = CurrentDrugTest.IncidentReportId;
                FollowUpTestDate = CurrentDrugTest.FollowUpTestDate;
                IsFollowUpComplete = CurrentDrugTest.IsFollowUpComplete;
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
                await UpdateDrugTestAsync();
            }
            else
            {
                await AddDrugTestAsync();
            }

            // Close the window or navigate back as appropriate
            CloseRequested?.Invoke(this, new DialogCloseRequestedEventArgs(true));
        }

        private async Task AddDrugTestAsync()
        {
            try
            {
                var newDrugTestDto = _mapper.Map<DrugTestCreateDto>(this);

                var result = await _apiClient.PostAsync<OperationResult>("drug-test/create", newDrugTestDto);
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

        private async Task UpdateDrugTestAsync()
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

