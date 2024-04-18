using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Net.Mime;
using System.Windows.Input;
using AutoMapper;
using TCMS.Common.DTOs.DrugTest;
using TCMS.Common.Operations;
using TCMS.Data.Models;
using TCMS.GUI.Models;
using TCMS.GUI.Services.Interfaces;
using TCMS.GUI.Utilities;
using TCMS.GUI.Views;
using Xceed.Wpf.Toolkit;
using DrugTest = TCMS.GUI.Models.DrugTest;

namespace TCMS.GUI.ViewModels
{
    public class DrugTestViewModel : ViewModelBase
    {
        // Example property representing a list of products
        private readonly IApiClient _apiClient;
        private readonly IMapper _mapper;
        private readonly IDialogService _dialogService;

        private Lazy<Task> _LazyDrugTestLoader;

        private ObservableCollection<DrugTest> _DrugTests;
        private ObservableCollection<DrugTest> _filteredDrugTests;

        public ObservableCollection<DrugTest> FilteredDrugTests
        {
            get => _filteredDrugTests;
            private set
            {
                _filteredDrugTests = value;
                OnPropertyChanged();
            }
        }

        private DrugTest _selectedDrugTest;

        private string _searchText = "Search Products...";

        public DrugTest SelectedDrugTest
        {
            get { return _selectedDrugTest; }
            set { _selectedDrugTest = value; OnPropertyChanged(); }
        }

        public string SearchText
        {
            get => _searchText;
            set
            {
                _searchText = value;
                OnPropertyChanged();
                // Try to ensure the products are loaded before filtering.
                _ = EnsureDrugTestsLoadedAsync().ContinueWith(t =>
                {
                    if (t.IsCompletedSuccessfully)
                    {
                        // Products are loaded, perform the filter.
                        FilterDrugTests();
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

        private void FilterDrugTests()
        {
            Console.WriteLine("FilterProducts called");

            if (_DrugTests == null)
            {
                Console.WriteLine("Products collection is null, loading products.");
                EnsureDrugTestsLoadedAsync();
                return;
            }

            Console.WriteLine($"Current SearchText: '{SearchText}'");
            if (string.IsNullOrEmpty(SearchText) || SearchText == "Search Products...")
            {
                Console.WriteLine("SearchText is empty or placeholder text, setting filteredProducts to all products.");
                FilteredDrugTests = new ObservableCollection<DrugTest>(_DrugTests);
            }
            else
            {
                Console.WriteLine("Searching...");
                // Determine if search text is purely numeric
                bool isNumericSearch = int.TryParse(SearchText, out int searchId);
                Console.WriteLine($"isNumericSearch: {isNumericSearch}, searchId: {searchId}");

                var filtered = _DrugTests.Where(drugtest =>
                    (isNumericSearch && drugtest.IncidentReportId.ToString().Contains(SearchText))) 
                    //(isNumericSearch && incident.IncidentDate.ToString().Contains(SearchText)) ||
                    //(!isNumericSearch && incident.Location.IndexOf(SearchText, StringComparison.OrdinalIgnoreCase) >= 0))
                  .ToList();

                Console.WriteLine($"Found {filtered.Count} products after filtering.");
                FilteredDrugTests = new ObservableCollection<DrugTest>(filtered);
            }

            Console.WriteLine($"FilteredProducts count after filtering: {FilteredDrugTests.Count}");
        }


        private void PerformSearch()
        {
            FilterDrugTests();
        }

        // Commands
        public ICommand AddDrugTestCommand { get; private set; }
        public ICommand EditDrugTestCommand { get; private set; }
        public ICommand DeleteDrugTestCommand { get; private set; }
        public ICommand RefreshDrugTestsCommand { get; private set; }
        public ICommand SearchCommand { get; }
        public ICommand SearchBoxGotFocusCommand { get; }
        public ICommand SearchBoxLostFocusCommand { get; }

        public DrugTestViewModel(IApiClient apiClient, IMapper mapper, IDialogService dialogService)
        {
            _apiClient = apiClient;
            _mapper = mapper;
            _dialogService = dialogService;
            // Initialize commands
            AddDrugTestCommand = new RelayCommand(AddDrugTest);
            EditDrugTestCommand = new RelayCommand(EditDrugTest, CanExecuteEditOrDelete);
            DeleteDrugTestCommand = new RelayCommand(EditDrugTest, CanExecuteEditOrDelete);
            RefreshDrugTestsCommand = new RelayCommand(RefreshDrugTests);
            SearchCommand = new RelayCommand((obj) => FilterDrugTests());
            SearchBoxGotFocusCommand = new RelayCommand(SearchBoxGotFocus);
            SearchBoxLostFocusCommand = new RelayCommand(SearchBoxLostFocus);

            _DrugTests = new ObservableCollection<DrugTest>();
            _filteredDrugTests = new ObservableCollection<DrugTest>();

            InitializeLazyLoader();

        }

        private void InitializeLazyLoader()
        {
            _LazyDrugTestLoader = new Lazy<Task>(() => LoadDrugTestAsync(), isThreadSafe: true);
        }

        private async Task LoadDrugTestAsync()
        {
            try
            {
                var result = await _apiClient.GetAsync<OperationResult<IEnumerable<DrugTest>>>("drug-test/all");
                if (result.IsSuccessful && result.Data != null)
                {
                    var drugtests = _mapper.Map<IEnumerable<DrugTest>>(result.Data);
                    App.Current.Dispatcher.Invoke(() =>
                    {
                        _DrugTests.Clear();
                        foreach (var drugtest in drugtests)
                        {
                            _DrugTests.Add(drugtest);
                        }
                        FilterDrugTests();
                    });
                }
                else
                {
                    // Log failure or handle accordingly
                    Debug.WriteLine("Failed to load products or no products returned.");
                }
            }
            catch (Exception ex)
            {
                // Log or handle exceptions
                Debug.WriteLine($"An error occurred while loading products: {ex.Message}");
            }
        }


        public async Task EnsureDrugTestsLoadedAsync()
        {
            if (!_LazyDrugTestLoader.IsValueCreated)
            {
                await _LazyDrugTestLoader.Value;
            }
        }

        private void AdjustSearchTextOnFocus()
        {
            if (_isSearchBoxFocused && SearchText == "Search Products...")
            {
                SearchText = string.Empty;
            }
            else if (!_isSearchBoxFocused && string.IsNullOrWhiteSpace(SearchText))
            {
                SearchText = "Search Products...";
            }
        }

        private void SearchBoxGotFocus(object obj)
        {
            if (SearchText == "Search Products...")
            {
                SearchText = string.Empty;
            }
        }

        private void SearchBoxLostFocus(object obj)
        {
            if (string.IsNullOrWhiteSpace(SearchText))
            {
                SearchText = "Search Products...";
            }
        }


        private void AddDrugTest(object obj)
        {
            var newDrugTestForm = new DrugTestFormViewModel(_apiClient, _mapper);
            newDrugTestForm.DrugTestUpdated += OnDrugTestUpdated;
            _dialogService.ShowDialog(newDrugTestForm);
            newDrugTestForm.DrugTestUpdated -= OnDrugTestUpdated;
        }

        private void OnDrugTestUpdated(object sender, EventArgs e)
        {
            RefreshDrugTests(null);
        }


        private void Refresh()
        {
            _ = LoadDrugTestAsync();
        }

        private void EditDrugTest(object obj)
        {
            if (SelectedDrugTest != null)
            {
                //var editDrugTestLogForm = new DrugTestFormViewModel(_apiClient, _mapper, SelectedDrugTest);
                //editDrugTestLogForm.DrugTestUpdated += OnDrugTestUpdated;
                //_dialogService.ShowDialog(editDrugTestLogForm);
                //editDrugTestLogForm.Cleanup();
                //editDrugTestLogForm.DrugTestUpdated -= OnDrugTestUpdated;
            }
        }

        private bool CanExecuteEditOrDelete(object obj)
        {
            // Determine if the edit/delete command can execute, e.g., based on if a product is selected
            return true; // Example logic
        }

        private void DeleteDrugTest(object obj)
        {
            // Implementation for deleting a selected product
        }

        private void RefreshDrugTests(object obj)
        {
            Refresh();
        }
    }
}

