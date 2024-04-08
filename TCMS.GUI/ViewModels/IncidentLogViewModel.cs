using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Net.Mime;
using System.Windows.Input;
using AutoMapper;
using TCMS.Common.DTOs.Incident;
using TCMS.Common.Operations;
using TCMS.Data.Models;
using TCMS.GUI.Models;
using TCMS.GUI.Services.Interfaces;
using TCMS.GUI.Utilities;
using TCMS.GUI.Views;
using Xceed.Wpf.Toolkit;
using IncidentReport = TCMS.GUI.Models.IncidentReport;

namespace TCMS.GUI.ViewModels
{
    public class IncidentLogViewModel : ViewModelBase
    {
        // Example property representing a list of products
        private readonly IApiClient _apiClient;
        private readonly IMapper _mapper;
        private readonly IDialogService _dialogService;

        private Lazy<Task> _LazyIncidentLoader;

        private ObservableCollection<IncidentReport> _Incidents;
        private ObservableCollection<IncidentReport> _filteredIncidents;

        public ObservableCollection<IncidentReport> FilteredIncidents
        {
            get => _filteredIncidents;
            private set
            {
                _filteredIncidents = value;
                OnPropertyChanged();
            }
        }

        private IncidentReport _selectedIncident;

        private string _searchText = "Search Products...";

        public IncidentReport SelectedIncident
        {
            get { return _selectedIncident; }
            set { _selectedIncident = value; OnPropertyChanged(); }
        }

        public string SearchText
        {
            get => _searchText;
            set
            {
                _searchText = value;
                OnPropertyChanged();
                // Try to ensure the products are loaded before filtering.
                _ = EnsureIncidentsLoadedAsync().ContinueWith(t =>
                {
                    if (t.IsCompletedSuccessfully)
                    {
                        // Products are loaded, perform the filter.
                        FilterIncidents();
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

        private void FilterIncidents()
        {
            Console.WriteLine("FilterProducts called");

            if (_Incidents == null)
            {
                Console.WriteLine("Products collection is null, loading products.");
                EnsureIncidentsLoadedAsync();
                return;
            }

            Console.WriteLine($"Current SearchText: '{SearchText}'");
            if (string.IsNullOrEmpty(SearchText) || SearchText == "Search Products...")
            {
                Console.WriteLine("SearchText is empty or placeholder text, setting filteredProducts to all products.");
                FilteredIncidents = new ObservableCollection<IncidentReport>(_Incidents);
            }
            else
            {
                Console.WriteLine("Searching...");
                // Determine if search text is purely numeric
                bool isNumericSearch = int.TryParse(SearchText, out int searchId);
                Console.WriteLine($"isNumericSearch: {isNumericSearch}, searchId: {searchId}");

                var filtered = _Incidents.Where(incident =>
                    (isNumericSearch && incident.IncidentReportId.ToString().Contains(SearchText)) ||
                    (isNumericSearch && incident.IncidentDate.ToString().Contains(SearchText)) ||
                    (!isNumericSearch && incident.Location.IndexOf(SearchText, StringComparison.OrdinalIgnoreCase) >= 0)) 
                  .ToList();

                Console.WriteLine($"Found {filtered.Count} products after filtering.");
                FilteredIncidents = new ObservableCollection<IncidentReport>(filtered);
            }

            Console.WriteLine($"FilteredProducts count after filtering: {FilteredIncidents.Count}");
        }


        private void PerformSearch()
        {
            FilterIncidents();
        }

        // Commands
        public ICommand AddIncidentCommand { get; private set; }
        public ICommand EditIncidentCommand { get; private set; }
        public ICommand DeleteIncidentCommand { get; private set; }
        public ICommand RefreshIncidentsCommand { get; private set; }
        public ICommand SearchCommand { get; }
        public ICommand SearchBoxGotFocusCommand { get; }
        public ICommand SearchBoxLostFocusCommand { get; }

        public IncidentLogViewModel(IApiClient apiClient, IMapper mapper, IDialogService dialogService)
        {
            _apiClient = apiClient;
            _mapper = mapper;
            _dialogService = dialogService;
            // Initialize commands
            AddIncidentCommand = new RelayCommand(AddIncident);
            EditIncidentCommand = new RelayCommand(EditIncident, CanExecuteEditOrDelete);
            DeleteIncidentCommand = new RelayCommand(DeleteIncident, CanExecuteEditOrDelete);
            RefreshIncidentsCommand = new RelayCommand(RefreshIncidents);
            SearchCommand = new RelayCommand((obj) => FilterIncidents());
            SearchBoxGotFocusCommand = new RelayCommand(SearchBoxGotFocus);
            SearchBoxLostFocusCommand = new RelayCommand(SearchBoxLostFocus);

            _Incidents = new ObservableCollection<IncidentReport>();
            _filteredIncidents = new ObservableCollection<IncidentReport>();

            InitializeLazyLoader();

        }

        private void InitializeLazyLoader()
        {
            _LazyIncidentLoader = new Lazy<Task>(() => LoadIncidentAsync(), isThreadSafe: true);
        }

        private async Task LoadIncidentAsync()
        {
            try
            {
                var result = await _apiClient.GetAsync<OperationResult<IEnumerable<IncidentReportDto>>>("incident/all");
                if (result.IsSuccessful && result.Data != null)
                {
                    var incidents = _mapper.Map<IEnumerable<IncidentReport>>(result.Data);
                    App.Current.Dispatcher.Invoke(() =>
                    {
                        _Incidents.Clear();
                        foreach (var incident in incidents)
                        {
                            _Incidents.Add(incident);
                        }
                        FilterIncidents();
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


        public async Task EnsureIncidentsLoadedAsync()
        {
            if (!_LazyIncidentLoader.IsValueCreated)
            {
                await _LazyIncidentLoader.Value;
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


        private void AddIncident(object obj)
        {
            var newIncidentLogForm = new IncidentLogFormViewModel(_apiClient, _mapper);
            newIncidentLogForm.IncidentUpdated += OnIncidentUpdated;
            _dialogService.ShowDialog(newIncidentLogForm);
            newIncidentLogForm.IncidentUpdated -= OnIncidentUpdated;
        }

        private void OnIncidentUpdated(object sender, EventArgs e)
        {
            RefreshIncidents(null);
        }


        private void Refresh()
        {
            _ = LoadIncidentAsync();
        }

        private void EditIncident(object obj)
        {
            if (SelectedIncident != null)
            {
                var editIncidentLogForm = new IncidentLogFormViewModel(_apiClient, _mapper, SelectedIncident);
                editIncidentLogForm.IncidentUpdated += OnIncidentUpdated;
                _dialogService.ShowDialog(editIncidentLogForm);
                editIncidentLogForm.Cleanup();
                editIncidentLogForm.IncidentUpdated -= OnIncidentUpdated;
            }
        }

        private bool CanExecuteEditOrDelete(object obj)
        {
            // Determine if the edit/delete command can execute, e.g., based on if a product is selected
            return true; // Example logic
        }

        private void DeleteIncident(object obj)
        {
            // Implementation for deleting a selected product
        }

        private void RefreshIncidents(object obj)
        {
            Refresh();
        }
    }
}

