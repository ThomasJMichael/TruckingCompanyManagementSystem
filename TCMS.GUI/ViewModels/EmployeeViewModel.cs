using AutoMapper;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using TCMS.Common.DTOs.Inventory;
using TCMS.Common.Operations;
using TCMS.GUI.Models;
using TCMS.GUI.Services.Interfaces;
using TCMS.GUI.Utilities;

namespace TCMS.GUI.ViewModels
{
    public class EmployeeViewModel : ViewModelBase
    {
        // Example property representing a list of products
        private readonly IApiClient _apiClient;
        private readonly IMapper _mapper;
        private readonly IDialogService _dialogService;
        private readonly IEmployeeUserService _employeeUserService;

        private Lazy<Task> _LazyEmployeesLoader;

        private ObservableCollection<Employee> _employees;
        private ObservableCollection<Employee> _filteredEmployees;

        public ObservableCollection<Employee> FilteredEmployees
        {
            get => _filteredEmployees;
            private set
            {
                _filteredEmployees = value;
                OnPropertyChanged();
            }
        }

        private Employee _selectedEmployee;
        public Employee SelectedEmployee
        {
            get { return _selectedEmployee; }
            set { _selectedEmployee = value; OnPropertyChanged(); }
        }

        private string _searchText = "Search Employees...";
        public string SearchText
        {
            get => _searchText;
            set
            {
                _searchText = value;
                OnPropertyChanged();
                // Try to ensure the products are loaded before filtering.
                _ = EnsureEmployeesLoadedAsync().ContinueWith(t =>
                {
                    if (t.IsCompletedSuccessfully)
                    {
                        // Products are loaded, perform the filter.
                        FilterEmployees();
                    }
                    // If the task is not completed, it's either still running or failed.
                    // If it's still running, the search will be triggered once it completes.
                    // If it failed, may want to log the error or inform the user as needed.
                }, TaskScheduler.FromCurrentSynchronizationContext());
            }
        }

        private bool _isSearchBoxFocused;

        public bool IsSearchBoxFocused
        {
            get => _isSearchBoxFocused;
            set
            {
                _isSearchBoxFocused = value;
                AdjustSearchTextOnFocus();
                OnPropertyChanged();
            }
        }

        private void FilterEmployees()
        {

            if (_employees == null)
            {
                Console.WriteLine("Employee collection is null, loading employees.");
                EnsureEmployeesLoadedAsync();
                return;
            }

            Console.WriteLine($"Current SearchText: '{SearchText}'");
            if (string.IsNullOrEmpty(SearchText) || SearchText == "Search Employees...")
            {
                FilteredEmployees = new ObservableCollection<Employee>(_employees);
            }
            else
            {
                Console.WriteLine("Searching...");
                // Determine if search text is purely numeric
                bool isNumericSearch = int.TryParse(SearchText, out int searchId);
                Console.WriteLine($"isNumericSearch: {isNumericSearch}, searchId: {searchId}");

                var filtered = _employees.Where(emp =>
                    (!isNumericSearch &&
                     (emp.FullName.IndexOf(SearchText, StringComparison.OrdinalIgnoreCase) >= 0 ||
                      (emp.UserRole != null &&
                       emp.UserRole.IndexOf(SearchText, StringComparison.OrdinalIgnoreCase) >= 0))))
                    .ToList();

                Console.WriteLine($"Found {filtered.Count} employees after filtering.");
                FilteredEmployees = new ObservableCollection<Employee>(filtered);
            }

            Console.WriteLine($"FilteredEmployees count after filtering: {FilteredEmployees.Count}");
        }


        private void PerformSearch()
        {
            FilterEmployees();
        }

        // Commands
        public ICommand AddEmployeeCommand { get; private set; }
        public ICommand EditEmployeeCommand { get; private set; }
        public ICommand DeleteEmployeeCommand { get; private set; }
        public ICommand RefreshEmployeesCommand { get; private set; }
        public ICommand SearchCommand { get; }
        public ICommand SearchBoxGotFocusCommand { get; }
        public ICommand SearchBoxLostFocusCommand { get; }

        public EmployeeViewModel(IApiClient apiClient, IMapper mapper, IDialogService dialogService, IEmployeeUserService employeeUserService)
        {
            _apiClient = apiClient;
            _mapper = mapper;
            _dialogService = dialogService;
            _employeeUserService = employeeUserService;

            // Initialize commands
            AddEmployeeCommand = new RelayCommand(AddEmployee);
            EditEmployeeCommand = new RelayCommand(EditEmployee, CanExecuteEditOrDelete);
            DeleteEmployeeCommand = new RelayCommand(DeleteEmployee, CanExecuteEditOrDelete);
            RefreshEmployeesCommand = new RelayCommand(RefreshEmployees);
            SearchCommand = new RelayCommand((obj) => FilterEmployees());
            SearchBoxGotFocusCommand = new RelayCommand(SearchBoxGotFocus);
            SearchBoxLostFocusCommand = new RelayCommand(SearchBoxLostFocus);

            _employees = new ObservableCollection<Employee>();
            _filteredEmployees = new ObservableCollection<Employee>();

            InitializeLazyLoader();

        }

        private void InitializeLazyLoader()
        {
            _LazyEmployeesLoader = new Lazy<Task>(() => LoadEmployeesAsync(), isThreadSafe: true);
        }

        private async Task LoadEmployeesAsync()
        {
            try
            {
                var results =  await _employeeUserService.GetEmployeesWithUserAccountsAsync();
                if (results != null)
                {
                    _employees = new ObservableCollection<Employee>(results);
                    FilterEmployees();
                }
                else
                {
                    Debug.WriteLine("Failed to load employees or no employees returned.");
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"An error occurred while loading products: {ex.Message}");
            }
        }


        public async Task EnsureEmployeesLoadedAsync()
        {
            if (!_LazyEmployeesLoader.IsValueCreated)
            {
                await _LazyEmployeesLoader.Value;
            }
        }

        private void AdjustSearchTextOnFocus()
        {
            if (_isSearchBoxFocused && SearchText == "Search Employees...")
            {
                SearchText = string.Empty;
            }
            else if (!_isSearchBoxFocused && string.IsNullOrWhiteSpace(SearchText))
            {
                SearchText = "Search Employees...";
            }
        }

        private void SearchBoxGotFocus(object obj)
        {
            if (SearchText == "Search Employees...")
            {
                SearchText = string.Empty;
            }
        }

        private void SearchBoxLostFocus(object obj)
        {
            if (string.IsNullOrWhiteSpace(SearchText))
            {
                SearchText = "Search Employees...";
            }
        }


        private void AddEmployee(object obj)
        {
            var newProductForm = new ProductFormViewModel(_apiClient, _mapper);
            newProductForm.ProductUpdated += OnProductUpdated;
            _dialogService.ShowDialog(newProductForm);
            newProductForm.ProductUpdated -= OnProductUpdated;
        }

        private void OnProductUpdated(object sender, EventArgs e)
        {
            RefreshEmployees(null);
        }


        private void Refresh()
        {
            _ = LoadEmployeesAsync();
        }

        private void EditEmployee(object obj)
        {
            if (SelectedEmployee != null)
            {
                // var editProductFormViewModel = new ProductFormViewModel(_apiClient, _mapper, SelectedProduct);
                //editProductFormViewModel.ProductUpdated += OnProductUpdated;
                //_dialogService.ShowDialog(editProductFormViewModel);
                //editProductFormViewModel.Cleanup();
                //editProductFormViewModel.ProductUpdated -= OnProductUpdated;
            }
        }

        private bool CanExecuteEditOrDelete(object obj)
        {
            // Determine if the edit/delete command can execute, e.g., based on if a product is selected
            return true; // Example logic
        }

        private void DeleteEmployee(object obj)
        {
            // Implementation for deleting a selected product
        }

        private void RefreshEmployees(object obj)
        {
            Refresh();
        }
    }
}
